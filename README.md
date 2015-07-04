# Spectrum
Spectrum - Searchable PDF Creator

Open-Source tool to generate searchable PDF files from large document libraries.
Uses Tesseract Engine and .Net-Framework. Licensed under GPLv2

Installation manual:

Tesseract OCR Engine installation

	1. At first the Tesseract OCR-Engine must be installed. The Tesseract engine is  responsible for the optical character recognition within the project. The current version of Tesseract can be downloaded here:
	https://code.google.com/p/tesseract-ocr/downloads/detail?name=tesseract-ocr-setup-3.02.02.exe&can=2&q=
	
Install additional language packages
	2. Unfortunately the Tesseract base package only installs English as recognition language. To install additional languages the respective language package must be downloaded. An overview of all available language packages can be found here:
	https://code.google.com/p/tesseract-ocr/downloads/list
	
	New language packages have to be installed manually: The downloaded archive has to be unzipped with 7-Zip. In the unzipped directory is a file with the ending .traineddata. This file contains the language data.
	
	Now the .traineddata-file must be moved in the "tessdata"-directory of the Tesseract-Engine. This directory can usually be found at "C:\Program Files\Tesseract OCR\tessdata".
	
Installation of Tesseract UI
	3. Tesseract UI can be installed easily with a simple .msi-installer. It can be downloaded here:
