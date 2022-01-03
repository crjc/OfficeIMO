﻿using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeIMO.Helper;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using TabStop = DocumentFormat.OpenXml.Wordprocessing.TabStop;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace OfficeIMO.Examples {
    internal class Program {
        private static void Setup(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            } else {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }

        static void Main(string[] args) {
            //string folderPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "documents");
            string templatesPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Documents");
            string folderPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Documents");
            Setup(folderPath);
            string filePath;

            //Console.WriteLine("[*] Creating standard document (empty)");
            //string filePath = System.IO.Path.Combine(folderPath, "EmptyDocument.docx");
            //Example_BasicEmptyWord(filePath, false);

            //Console.WriteLine("[*] Creating standard document with paragraph");
            //filePath = System.IO.Path.Combine(folderPath, "BasicDocumentWithParagraphs.docx");
            //Example_BasicWord(filePath, true);

            //Console.WriteLine("[*] Creating standard document with some properties and single paragraph");
            //filePath = System.IO.Path.Combine(folderPath, "BasicDocument.docx");
            //Example_BasicDocumentProperties(filePath, false);

            //Console.WriteLine("[*] Creating standard document with multiple paragraphs, with some formatting");
            //filePath = System.IO.Path.Combine(folderPath, "AdvancedParagraphs.docx");
            //Example_MultipleParagraphsViaDifferentWays(filePath, false);

            //Console.WriteLine("[*] Creating standard document with some Images");
            //filePath = System.IO.Path.Combine(folderPath, "BasicDocumentWithImages.docx");
            //Example_AddingImages(filePath, false);

            Console.WriteLine("[*] Read Basic Word");
            Example_ReadWord(true);

            //Console.WriteLine("[*] Read Basic Word with Images");
            //Example_ReadWordWithImages();

            //Console.WriteLine("[*] Creating standard document with page breaks and removing them");
            //filePath = System.IO.Path.Combine(folderPath, "Basic Document with some page breaks.docx");
            //Example_PageBreaks(filePath, true);

            //Console.WriteLine("[*] Creating standard document with Sections");
            //filePath = System.IO.Path.Combine(folderPath, "Basic Document with Sections.docx");
            //Example_BasicWordWithSections(filePath, true);

            //Console.WriteLine("[*] Creating standard document with Headers and Footers");
            //filePath = System.IO.Path.Combine(folderPath, "Basic Document with PageOrientationChange.docx");
            //Example_PageOrientation(filePath, true);

            //Console.WriteLine("[*] Creating standard document with Headers and Footers");
            //filePath = System.IO.Path.Combine(folderPath, "Basic Document with Headers and Footers.docx");
            //Example_BasicWordWithHeaderAndFooter(filePath, true);

            //Console.WriteLine("[*] Loading basic document");
            //Example_Load(filePath, true);

            //OpenAndAddTextToWordDocument();
        }

        private static void Example_BasicEmptyWord(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create(filePath)) {
                document.Title = "This is my title";
                document.Creator = "Przemysław Kłys";
                document.Keywords = "word, docx, test";
                document.Save(openWord);
            }
        }

        private static void Example_BasicWord(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create(filePath)) {
                var paragraph = document.InsertParagraph("Basic paragraph");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                paragraph = document.InsertParagraph("2nd paragraph");
                paragraph.Bold = true;
                paragraph = paragraph.AppendText(" continue?");
                paragraph.Underline = UnderlineValues.DashLong;
                paragraph = paragraph.AppendText("More text");
                paragraph.Color = System.Drawing.Color.CornflowerBlue.ToHexColor();

                document.Save(openWord);
            }
        }

        private static void Example_BasicDocumentProperties(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create(filePath)) {
                document.Title = "This is my title";
                document.Creator = "Przemysław Kłys";
                document.Keywords = "word, docx, test";

                var paragraph = document.InsertParagraph("Basic paragraph");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                document.Save(openWord);
            }
        }

        private static void Example_MultipleParagraphsViaDifferentWays(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create()) {
                var paragraph = document.InsertParagraph("This paragraph starts with some text");
                paragraph.Bold = true;
                paragraph.Text = "0th This paragraph started with some other text and was overwritten and made bold.";

                paragraph = document.InsertParagraph("1st Test Second Paragraph");

                paragraph = document.InsertParagraph();
                paragraph.Text = "2nd Test Third Paragraph, ";
                paragraph.Underline = UnderlineValues.None;
                var paragraph2 = paragraph.AppendText("3rd continuing?");
                paragraph2.Underline = UnderlineValues.Double;
                paragraph2.Bold = true;
                paragraph2.Spacing = 200;

                document.InsertParagraph().InsertText("4th Fourth paragraph with text").Bold = true;

                WordParagraph paragraph1 = new WordParagraph() {
                    Text = "Fifth paragraph",
                    Italic = true,
                    Bold = true
                };
                document.InsertParagraph(paragraph1);

                paragraph = document.InsertParagraph("5th Test gmarmmar, this shouldnt show up as baddly written.");
                paragraph.DoNotCheckSpellingOrGrammar = true;
                paragraph.CapsStyle = CapsStyle.Caps;

                paragraph = document.InsertParagraph("6th Test gmarmmar, this should show up as baddly written.");
                paragraph.DoNotCheckSpellingOrGrammar = false;
                paragraph.CapsStyle = CapsStyle.SmallCaps;

                paragraph = document.InsertParagraph("7th Highlight me?");
                paragraph.Highlight = HighlightColorValues.Yellow;
                paragraph.FontSize = 15;
                paragraph.ParagraphAlignment = JustificationValues.Center;


                paragraph = document.InsertParagraph("8th This text should be colored.");
                paragraph.Bold = true;
                paragraph.Color = "4F48E2";
                paragraph.IndentationAfter = 1400;


                paragraph = document.InsertParagraph("This is very long line that we will use to show indentation that will work across multiple lines and more and more and even more than that. One, two, three, don't worry baby.");
                paragraph.Bold = true;
                paragraph.Color = "#FF0000";
                paragraph.IndentationBefore = 720;
                paragraph.IndentationFirstLine = 1400;


                paragraph = document.InsertParagraph("9th This text should be colored and Arial.");
                paragraph.Bold = true;
                paragraph.Color = "4F48E2";
                paragraph.FontFamily = "Arial";
                paragraph.VerticalCharacterAlignmentOnLine = VerticalTextAlignmentValues.Bottom;

                paragraph = document.InsertParagraph("10th This text should be colored and Tahoma.");
                paragraph.Bold = true;
                paragraph.Color = "4F48E2";
                paragraph.FontFamily = "Tahoma";
                paragraph.FontSize = 20;
                paragraph.LineSpacingBefore = 300;

                paragraph = document.InsertParagraph("12th This text should be colored and Tahoma and text direction changed");
                paragraph.Bold = true;
                paragraph.Color = "4F48E2";
                paragraph.FontFamily = "Tahoma";
                paragraph.FontSize = 10;
                paragraph.TextDirection = TextDirectionValues.TopToBottomRightToLeftRotated;

                paragraph = document.InsertParagraph("Spacing Test 1");
                paragraph.Bold = true;
                paragraph.Color = "4F48E2";
                paragraph.FontFamily = "Tahoma";
                paragraph.LineSpacingAfter = 720;

                paragraph = document.InsertParagraph("Spacing Test 2");
                paragraph.Bold = true;
                paragraph.Color = "4F48E2";
                paragraph.FontFamily = "Tahoma";


                paragraph = document.InsertParagraph("Spacing Test 3");
                paragraph.Bold = true;
                paragraph.Color = "4F48E2";
                paragraph.FontFamily = "Tahoma";
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.LineSpacing = 1500;

                Console.WriteLine("Found paragraphs in document: " + document.Paragraphs.Count);

                document.Save(filePath, openWord);
            }
        }

        private static void Example_AddingImages(string filePath, bool openWord) {
            //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //string imagePaths = System.IO.Path.Combine(baseDirectory, "Images");
            string imagePaths = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Images");

            WordDocument document = WordDocument.Create(filePath);
            document.Title = "This is sparta";
            document.Creator = "Przemek";

            var paragraph = document.InsertParagraph("This paragraph starts with some text");
            paragraph.Text = "0th This paragraph started with some other text and was overwritten and made bold.";

            // lets add image to paragraph
            paragraph.InsertImage(System.IO.Path.Combine(imagePaths, "PrzemyslawKlysAndKulkozaurr.jpg"), 22, 22);
            //paragraph.Image.WrapText = true; // WrapSideValues.Both;

            var paragraph5 = paragraph.AppendText("and more text");
            paragraph5.Bold = true;


            document.InsertParagraph("This adds another picture with 500x500");

            var filePathImage = System.IO.Path.Combine(imagePaths, "Kulek.jpg");
            WordParagraph paragraph2 = document.InsertParagraph();
            paragraph2.InsertImage(filePathImage, 500, 500);
            //paragraph2.Image.BlackWiteMode = BlackWhiteModeValues.GrayWhite;
            paragraph2.Image.Rotation = 180;
            paragraph2.Image.Shape = ShapeTypeValues.ActionButtonMovie;


            document.InsertParagraph("This adds another picture with 100x100");

            WordParagraph paragraph3 = document.InsertParagraph();
            paragraph3.InsertImage(filePathImage, 100, 100);

            // we add paragraph with an image
            WordParagraph paragraph4 = document.InsertParagraph();
            paragraph4.InsertImage(filePathImage);

            // we can get the height of the image from paragraph
            Console.WriteLine("This document has image, which has height of: " + paragraph4.Image.Height + " pixels (I think) ;-)");

            // we can also overwrite height later on
            paragraph4.Image.Height = 50;
            paragraph4.Image.Width = 50;
            // this doesn't work
            paragraph4.Image.HorizontalFlip = true;

            // or we can get any image and overwrite it's size
            document.Images[0].Height = 200;
            document.Images[0].Width = 200;

            string fileToSave = System.IO.Path.Combine(imagePaths, "OutputPrzemyslawKlysAndKulkozaurr.jpg");
            document.Images[0].SaveToFile(fileToSave);

            document.Save(true);
        }

        private static void Example_ReadWord(bool openWord) {
            string documentPaths = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates");

            WordDocument document = WordDocument.Load(System.IO.Path.Combine(documentPaths, "BasicDocument.docx"), true);

            Console.WriteLine("This document has " + document.Paragraphs.Count + " paragraphs. Cool right?");
            Console.WriteLine("+ Document Title: " + document.Title);
            Console.WriteLine("+ Document Author: " + document.Creator);
            Console.WriteLine("+ FileOpen: " + document.FileOpenAccess);
            
            document.Dispose();
        }

        private static void Example_ReadWordWithImages() {
            string outputPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Documents");
            string documentPaths = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates");

            WordDocument document = WordDocument.Load(System.IO.Path.Combine(documentPaths, "BasicDocumentWithImages.docx"), true);
            Console.WriteLine("+ Document paragraphs: " + document.Paragraphs.Count);
            Console.WriteLine("+ Document images: " + document.Images.Count);

            document.Images[0].SaveToFile(System.IO.Path.Combine(outputPath, "random.jpg"));
        }

        private static void Example_PageBreaks(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create(filePath)) {
                document.Title = "This is my title";
                document.Creator = "Przemysław Kłys";
                document.Keywords = "word, docx, test";

                var paragraph = document.InsertParagraph("Test 1");

                //paragraph = new WordParagraph(document);
                //WordSection section = new WordSection(document, paragraph);


                //document._document.Body.Append(PageBreakParagraph);
                //document._document.Body.InsertBefore(PageBreakParagraph, paragraph._paragraph);

                document.InsertPageBreak();

                paragraph.Text = "Test 2";

                paragraph = document.InsertParagraph("Test 2");

                // Now lets remove paragraph with page break
                document.Paragraphs[1].Remove();

                // Now lets remove 1st paragraph
                document.Paragraphs[0].Remove();

                document.InsertPageBreak();

                document.InsertParagraph().Text = "Some text on next page";

                var paragraph1 = document.InsertParagraph("Test").AppendText("Test2");
                paragraph1.Color = System.Drawing.Color.Red.ToHexColor();
                paragraph1.AppendText("Test3");

                paragraph = document.InsertParagraph("Some paragraph");
                paragraph.Bold = true;
                paragraph = paragraph.AppendText(" continue?");
                paragraph.Underline = UnderlineValues.DashLong;

                document.InsertPageBreak();

                paragraph = document.InsertParagraph("Basic paragraph");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                paragraph = document.InsertParagraph("2nd paragraph");
                paragraph.Bold = true;
                paragraph = paragraph.AppendText(" continue?");
                paragraph.Underline = UnderlineValues.DashLong;
                paragraph = paragraph.AppendText(" More text");
                paragraph.Color = System.Drawing.Color.CornflowerBlue.ToHexColor();

                // remove last paragraph
                document.Paragraphs.Last().Remove();

                paragraph = document.InsertParagraph("2nd paragraph");
                paragraph.Bold = true;
                paragraph = paragraph.AppendText(" continue?");
                paragraph.Underline = UnderlineValues.DashLong;
                paragraph = paragraph.AppendText(" More text");
                paragraph.Color = System.Drawing.Color.CornflowerBlue.ToHexColor();

                // remove paragraph
                int countParagraphs = document.Paragraphs.Count;
                document.Paragraphs[countParagraphs - 2].Remove();

                // remove first page break
                document.PageBreaks[0].Remove();

                document.Save(openWord);
            }
        }

        private static void Example_BasicWordWithHeaderAndFooter1(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create(filePath)) {

                document.Sections[0].ColumnsSpace = 50;
                Console.WriteLine("+ Settings Zoom Preset: " + document.Settings.ZoomPreset);
                Console.WriteLine("+ Settings Zoom Percent: " + document.Settings.ZoomPercentage);

                //document.Settings.ZoomPreset = PresetZoomValues.BestFit;
                //document.Settings.ZoomPercentage = 30;

                Console.WriteLine("+ Settings Zoom Preset: " + document.Settings.ZoomPreset);
                Console.WriteLine("+ Settings Zoom Percent: " + document.Settings.ZoomPercentage);

                document.AddHeadersAndFooters();
                document.DifferentFirstPage = true;
                //document.DifferentOddAndEvenPages = false;
                //var paragraphInFooter = document.Footer.Default.InsertParagraph();
                //paragraphInFooter.Text = "This is a test on odd pages (aka default if no options are set)";

                var paragraphInHeader = document.Header.Default.InsertParagraph();
                paragraphInHeader.Text = "Default Header / Section 0";

                paragraphInHeader = document.Header.First.InsertParagraph();
                paragraphInHeader.Text = "First Header / Section 0";

                //var paragraphInFooterFirst = document.Footer.First.InsertParagraph();
                //paragraphInFooterFirst.Text = "This is a test on first";

                //var count = document.Footer.First.Paragraphs.Count;

                //var paragraphInFooterOdd = document.Footer.Odd.InsertParagraph();
                //paragraphInFooterOdd.Text = "This is a test odd";


                //var paragraphHeader = document.Header.Odd.InsertParagraph();
                //paragraphHeader.Text = "Header - ODD";

                //var paragraphInFooterEven = document.Footer.Even.InsertParagraph();
                //paragraphInFooterEven.Text = "This is a test - Even";


                var paragraph = document.InsertParagraph("Basic paragraph - Page 1");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                //paragraph = document.InsertPageBreak();

                paragraph = document.InsertParagraph("Basic paragraph - Page 2");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                //paragraph = document.InsertPageBreak();

                paragraph = document.InsertParagraph("Basic paragraph - Page 3");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                //paragraph = document.InsertPageBreak();

                paragraph = document.InsertParagraph("Basic paragraph - Page 4");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                //paragraph = document.InsertPageBreak();

                paragraph = document.InsertParagraph("Basic paragraph - Page 5");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                //var section2 = document.InsertSection(SectionMarkValues.NextPage);
                var section2 = document.InsertSection();
                section2.AddHeadersAndFooters();
                section2.DifferentFirstPage = true;
                

                // Add header to section
                //var paragraghInHeaderSection = section2.Header.First.InsertParagraph();
                //paragraghInHeaderSection.Text = "Ok, work please?";

                var paragraghInHeaderSection1 = section2.Header.Default.InsertParagraph();
                paragraghInHeaderSection1.Text = "Weird shit? 1";

                paragraghInHeaderSection1 = section2.Header.First.InsertParagraph();
                paragraghInHeaderSection1.Text = "Weird shit 2?";
               // paragraghInHeaderSection1.InsertText("ok?");

                paragraghInHeaderSection1 = section2.Header.Even.InsertParagraph();
                paragraghInHeaderSection1.Text = "Weird shit? 3";

                paragraph = document.InsertParagraph("Basic paragraph - Page 6");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                paragraph = document.InsertPageBreak();

                paragraph = document.InsertParagraph("Basic paragraph - Page 7");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();


                paragraph = document.InsertParagraph("Basic paragraph - Section 3.1");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                paragraph = document.InsertPageBreak();

                paragraph = document.InsertParagraph("Basic paragraph - Section 3.2");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                paragraph = document.InsertPageBreak();

                //paragraph = document.Footer.Odd.InsertParagraph();
                //paragraph.Text = "Lets see";

                // 2 section, 9 paragraphs + 7 pagebreaks = 15 paragraphs, 7 pagebreaks
                Console.WriteLine("+ Paragraphs: " + document.Paragraphs.Count);
                Console.WriteLine("+ PageBreaks: " + document.PageBreaks.Count);
                Console.WriteLine("+ Sections: " + document.Sections.Count);

                // primary section (for the whole document)
                Console.WriteLine("+ Paragraphs section 0: " + document.Sections[0].Paragraphs.Count);
                // additional sections
                Console.WriteLine("+ Paragraphs section 1: " + document.Sections[1].Paragraphs.Count);
                //Console.WriteLine("+ Paragraphs section 2: " + document.Sections[0].Paragraphs.Count);
                //Console.WriteLine("+ Paragraphs section 3: " + document.Sections[0].Paragraphs.Count);
                document.Save(openWord);
            }
        }

        private static void Example_PageOrientation(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create(filePath)) {
                Console.WriteLine("+ Page Orientation (starting): " + document.PageOrientation);

                document.Sections[0].PageOrientation = PageOrientationValues.Landscape;

                Console.WriteLine("+ Page Orientation (middle): " + document.PageOrientation);
                
                document.PageOrientation = PageOrientationValues.Portrait;

                Console.WriteLine("+ Page Orientation (ending): " + document.PageOrientation);

                document.InsertParagraph("Test");
                
                document.Save(openWord);
            }
        }
    

        private static void Example_BasicWordWithHeaderAndFooter(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create(filePath)) {
                document.AddHeadersAndFooters();

                document.Sections[0].PageOrientation = PageOrientationValues.Landscape;

                var paragraphInHeader = document.Header.Default.InsertParagraph();
                paragraphInHeader.Text = "Default Header / Section 0";
                
                document.InsertPageBreak();

                var paragraph = document.InsertParagraph("Basic paragraph - Page 1");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                var section2 = document.InsertSection();
                section2.AddHeadersAndFooters();
                
                var paragraghInHeaderSection1 = section2.Header.Default.InsertParagraph();
                paragraghInHeaderSection1.Text = "Weird shit? 1";
                
                paragraph = document.InsertParagraph("Basic paragraph - Page 2");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                var section3 = document.InsertSection();
                section3.AddHeadersAndFooters();

                var paragraghInHeaderSection3 = section3.Header.Default.InsertParagraph();
                paragraghInHeaderSection3.Text = "Weird shit? 2";

                paragraph = document.InsertParagraph("Basic paragraph - Page 3");
                paragraph.ParagraphAlignment = JustificationValues.Center;
                paragraph.Color = System.Drawing.Color.Red.ToHexColor();

                // 2 section, 9 paragraphs + 7 pagebreaks = 15 paragraphs, 7 pagebreaks
                Console.WriteLine("+ Paragraphs: " + document.Paragraphs.Count);
                Console.WriteLine("+ PageBreaks: " + document.PageBreaks.Count);
                Console.WriteLine("+ Sections: " + document.Sections.Count);

                // primary section (for the whole document)
                Console.WriteLine("+ Paragraphs section 0: " + document.Sections[0].Paragraphs.Count);
                // additional sections
                Console.WriteLine("+ Paragraphs section 1: " + document.Sections[1].Paragraphs.Count);
                //Console.WriteLine("+ Paragraphs section 2: " + document.Sections[0].Paragraphs.Count);
                //Console.WriteLine("+ Paragraphs section 3: " + document.Sections[0].Paragraphs.Count);
                document.Save(openWord);
            }
        }


        private static void Example_BasicWordWithSections(string filePath, bool openWord) {
            using (WordDocument document = WordDocument.Create(filePath)) {
                document.InsertParagraph("Test 1");
                var section1 = document.InsertSection(SectionMarkValues.NextPage);

                document.InsertParagraph("Test 2");
                var section2 = document.InsertSection(SectionMarkValues.Continuous);

                document.InsertParagraph("Test 3");
                var section3 = document.InsertSection(SectionMarkValues.NextPage);
                section3.InsertParagraph("Paragraph added to section number 3");
                section3.InsertParagraph("Continue adding paragraphs to section 3");

                // 4 section, 5 paragraphs, 0 pagebreaks
                Console.WriteLine("+ Paragraphs: " + document.Paragraphs.Count);
                Console.WriteLine("+ PageBreaks: " + document.PageBreaks.Count);
                Console.WriteLine("+ Sections: " + document.Sections.Count);

                // primary section (for the whole document)
                Console.WriteLine("+ Paragraphs section 0: " + document.Sections[0].Paragraphs.Count);
                // additional sections
                Console.WriteLine("+ Paragraphs section 1: " + document.Sections[1].Paragraphs.Count);
                Console.WriteLine("+ Paragraphs section 2: " + document.Sections[2].Paragraphs.Count);
                Console.WriteLine("+ Paragraphs section 3: " + document.Sections[3].Paragraphs.Count);

                // change same paragraph using section
                document.Sections[1].Paragraphs[0].Bold = true;
                // or Paragraphs list for the whole document
                document.Paragraphs[1].Color = "7178a8";

                var paragraph = section1.InsertParagraph("We missed paragraph on 1 section (2nd page)");
                var newParagraph = paragraph.InsertParagraphAfterSelf();
                newParagraph.Text = "Some more text, after paragraph we just added.";
                newParagraph.Bold = true;


                Console.WriteLine("+ Paragraphs (repeated): " + document.Paragraphs.Count);
                Console.WriteLine("+ PageBreaks (repeated): " + document.PageBreaks.Count);
                Console.WriteLine("+ Sections   (repeated): " + document.Sections.Count);
                // primary section (for the whole document)
                Console.WriteLine("+ Paragraphs section 0 (repeated): " + document.Sections[0].Paragraphs.Count);
                // additional sections
                Console.WriteLine("+ Paragraphs section 1 (repeated): " + document.Sections[1].Paragraphs.Count);
                Console.WriteLine("+ Paragraphs section 2 (repeated): " + document.Sections[2].Paragraphs.Count);
                Console.WriteLine("+ Paragraphs section 3 (repeated): " + document.Sections[3].Paragraphs.Count);


                document.Save(openWord);
            }
        }

        private static void Example_Load(string filePath, bool openWord) {
            filePath = @"C:\Support\GitHub\OfficeIMO\OfficeIMO.Tests\Documents\DocumentWithSection.docx";
            //filePath = @"C:\Support\GitHub\OfficeIMO\OfficeIMO.Tests\Documents\EmptyDocumentWithSection.docx";

            using (WordDocument document = WordDocument.Load(filePath, true)) {
                Console.WriteLine("+ Document Path: " + document.FilePath);
                Console.WriteLine("+ Document Title: " + document.Title);
                Console.WriteLine("+ Document Author: " + document.Creator);

                Console.WriteLine("+ Paragraphs: " + document.Paragraphs.Count);
                Console.WriteLine("+ PageBreaks: " + document.PageBreaks.Count);
                Console.WriteLine("+ Sections: " + document.Sections.Count);

                document.Dispose();
            }
        }


        public static void OpenAndAddTextToWordDocument() {
            string path = @"C:\Users\przemyslaw.klys\OneDrive - Evotec\Desktop\TEstoor.docx";
            string strtxt = "OpenXML SDK";
            // Open a WordprocessingDocument for editing using the filepath.
           WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(path, true);
            //WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document);

            MainDocumentPart part = wordprocessingDocument.MainDocumentPart;
            Body body = part.Document.Body;
            //create a new footer Id=rIdf2
            FooterPart footerPart2 = part.AddNewPart<FooterPart>("rIdf2");
            GenerateFooterPartContent(footerPart2);
            //create a new header Id=rIdh2
            HeaderPart headerPart2 = part.AddNewPart<HeaderPart>("rIdh2");
            GenerateHeaderPartContent(headerPart2);
            //replace the attribute of SectionProperties to add new footer and header
            SectionProperties lxml = body.GetFirstChild<SectionProperties>();

           lxml.GetFirstChild<HeaderReference>().Remove();
            lxml.GetFirstChild<FooterReference>().Remove();
            HeaderReference headerReference1 = new HeaderReference() {Type = HeaderFooterValues.Default, Id = "rIdh2"};
            FooterReference footerReference1 = new FooterReference() {Type = HeaderFooterValues.Default, Id = "rIdf2"};
            lxml.Append(headerReference1);
            lxml.Append(footerReference1);
            //add the correlation of last Paragraph
            OpenXmlElement oxl = body.ChildElements.GetItem(body.ChildElements.Count - 2);
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            //SectionProperties sectionProperties1 = new SectionProperties() {RsidR = oxl.GetAttribute("rsidR", oxl.NamespaceUri).Value};
            SectionProperties sectionProperties1 = new SectionProperties() {  };
            HeaderReference headerReference2 = new HeaderReference() {Type = HeaderFooterValues.Default, Id = part.GetIdOfPart(part.HeaderParts.FirstOrDefault())};
            FooterReference footerReference2 = new FooterReference() {Type = HeaderFooterValues.Default, Id = part.GetIdOfPart(part.FooterParts.FirstOrDefault())};
            PageSize pageSize1 = new PageSize() {Width = (UInt32Value) 12240U, Height = (UInt32Value) 15840U};
            PageMargin pageMargin1 = new PageMargin() {Top = 1440, Right = (UInt32Value) 1440U, Bottom = 1440, Left = (UInt32Value) 1440U, Header = (UInt32Value) 720U, Footer = (UInt32Value) 720U, Gutter = (UInt32Value) 0U};
            Columns columns1 = new Columns() {Space = "720"};
            DocGrid docGrid1 = new DocGrid() {LinePitch = 360};
            sectionProperties1.Append(headerReference2);
            sectionProperties1.Append(footerReference2);
            sectionProperties1.Append(pageSize1);
            sectionProperties1.Append(pageMargin1);
            sectionProperties1.Append(columns1);
            sectionProperties1.Append(docGrid1);
            paragraphProperties1.Append(sectionProperties1);
            oxl.InsertAt<ParagraphProperties>(paragraphProperties1, 0);
            body.InsertBefore<Paragraph>(GenerateParagraph(strtxt, oxl.GetAttribute("rsidRDefault", oxl.NamespaceUri).Value), body.GetFirstChild<SectionProperties>());
            part.Document.Save();
            wordprocessingDocument.Close();
        }

        //Generate new Paragraph
        public static Paragraph GenerateParagraph(string text, string rsidR) {
            Paragraph paragraph1 = new Paragraph() {RsidParagraphAddition = rsidR};
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            Tabs tabs1 = new Tabs();
            TabStop tabStop1 = new TabStop() {Val = TabStopValues.Left, Position = 5583};
            tabs1.Append(tabStop1);
            paragraphProperties1.Append(tabs1);
            Run run1 = new Run();
            Text text1 = new Text();
            text1.Text = text;
            run1.Append(text1);
            Run run2 = new Run();
            TabChar tabChar1 = new TabChar();
            run2.Append(tabChar1);
            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);
            paragraph1.Append(run2);
            return paragraph1;
        }

        static void GenerateHeaderPartContent(HeaderPart hpart) {
            Header header1 = new Header();
            Paragraph paragraph1 = new Paragraph();
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() {Val = "Header"};
            paragraphProperties1.Append(paragraphStyleId1);
            Run run1 = new Run();
            Text text1 = new Text();
            text1.Text = "";
            run1.Append(text1);
            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);
            header1.Append(paragraph1);
            hpart.Header = header1;
        }

        static void GenerateFooterPartContent(FooterPart fpart) {
            Footer footer1 = new Footer();
            Paragraph paragraph1 = new Paragraph();
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            ParagraphStyleId paragraphStyleId1 = new ParagraphStyleId() {Val = "Footer"};
            paragraphProperties1.Append(paragraphStyleId1);
            Run run1 = new Run();
            Text text1 = new Text();
            text1.Text = "";
            run1.Append(text1);
            paragraph1.Append(paragraphProperties1);
            paragraph1.Append(run1);
            footer1.Append(paragraph1);
            fpart.Footer = footer1;
        }
    }
}