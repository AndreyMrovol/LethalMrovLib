const axios = require("axios").default; // Note the .default for CommonJS
const fs = require("fs");
const path = require("path");
const extract = require("extract-zip");

console.warn(process.argv);
let argCount = process.argv.length - 1;

const urls = process.argv[argCount - 1].split(";");
const outputPaths = process.argv[argCount].split(";");

if (urls.length !== outputPaths.length) {
	console.error("The number of URLs must match the number of output paths.");
	process.exit(1);
}

const downloadFile = async (url, outputPath) => {
	const dir = path.dirname(outputPath);
	if (!fs.existsSync(dir)) {
		fs.mkdirSync(dir, { recursive: true });
	}

	if (fs.existsSync(outputPath)) {
		console.log(`File ${outputPath} already exists. Skipping download.`);
		return;
	}

	const response = await axios({
		method: "get",
		url: url,
		responseType: "arraybuffer",
	});

	fs.writeFileSync(outputPath, response.data);

	console.log(`Downloaded ${url} to ${outputPath}`);
};

const extractAndFilterDllFiles = async (zipPath, extractTo) => {
	try {
		const absExtractTo = path.resolve(extractTo); // Get absolute path for extraction

		await extract(zipPath, {
			dir: absExtractTo,
			onEntry: (entry) => {
				if (path.extname(entry.fileName) === ".dll") {
					entry.fileName = path.basename(entry.fileName); // Only keep the filename
					return entry;
				} else {
					entry.ignore = true; // Ignore non-.dll files
				}
			},
		});

		console.log(`Extracted and filtered .dll files from ${zipPath} to ${absExtractTo}`);

		// Remove non-.dll files
		const files = fs.readdirSync(absExtractTo);
		for (const file of files) {
			const filePath = path.join(absExtractTo, file);
			if (path.extname(file) !== ".dll") {
				fs.unlinkSync(filePath); // Delete non-.dll files
				console.log(`Deleted ${filePath}`);
			}
		}
	} catch (err) {
		console.error(`Error extracting ${zipPath}: ${err.message}`);
	}
};

const downloadAndExtractAllFiles = async () => {
	console.warn(urls);

	for (let i = 0; i < urls.length; i++) {
		try {
			const outputPath = outputPaths[i];
			await downloadFile(urls[i], outputPath);

			if (outputPath.endsWith(".zip")) {
				await extractAndFilterDllFiles(outputPath, path.dirname(outputPath));
			}
		} catch (err) {
			console.error(`Error processing ${urls[i]}: ${err.message}`);
		}
	}
};

downloadAndExtractAllFiles();
