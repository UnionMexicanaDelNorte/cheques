/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfImage
//	PDF Image resource.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013-2015 Granotech Limited. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	For version history please refer to PdfDocument.cs
//
/////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
/// <summary>
/// PDF Image class
/// </summary>
/// <remarks>
/// <para>
/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#ImageSupport">2.4 Image Support</a>
/// </para>
/// <para>
/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DrawImage">For example of drawing image see 3.9. Draw Image and Clip it</a>
/// </para>
/// </remarks>
public class PdfImage : PdfObject, IDisposable
	{
	/// <summary>
	/// Gets image width in pixels
	/// </summary>
	public	Int32			WidthPix {get; private set;}	// in pixels

	/// <summary>
	/// Gets image height in pixels
	/// </summary>
	public  Int32			HeightPix {get; private set;}	// in pixels

	private Int32			ImageQuality;
	private PdfObject		ImageLengthObject;
	private Bitmap			Picture;
	private Boolean			DisposePicture;

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor for image file
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="ImageFileName">Image file name</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional)</param>
	/// <param name="ImageQuality">Image quality 0 to 100 or -1 (optional)</param>
	/// <remarks>
	/// <para>Image quality is a parameter that used by the .net framework
	/// during the compression of the image from bitmap to jpeg. If the parameter
	/// is missing or set to -1 the library saves the bitmap image as</para>
	/// <code>
	///	Bitmap.Save(MemoryStream, ImageFormat.Jpeg);
	///	</code>
	///	<para>If the ImageQuality parameter is 0 to 100, the library saves the bitmap image as</para>
	///	<code>
	///	EncoderParameters EncoderParameters = new EncoderParameters(1);
	/// EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
	/// Bitmap.Save(MemoryStream, GetEncoderInfo("image/jpeg"), EncoderParameters);
	///	</code>
	///	<para>Microsoft does not specify the image quality factor used in the 
	///	first method of saving. However, experimantaion and Internet comments shows that it is 75.</para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfImage
			(
			PdfDocument		Document,
			String			ImageFileName,
			Double			Resolution = 0.0,		// pixels per inch
			Int32			ImageQuality = -1		// 0 to 100 or -1 for default save or -1 for default save
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		ConstructorHelper(ImageFileName, null, Rectangle.Empty, RectangleF.Empty, Resolution, ImageQuality);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor for image file
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="ImageFileName">Image file name</param>
	/// <param name="CropRect">Crop rectangle (in pixels)</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional)</param>
	/// <param name="ImageQuality">Image quality 0 to 100 or -1 (optional)</param>
	/// <remarks>
	/// <para>Image quality is a parameter that used by the .net framework
	/// during the compression of the image from bitmap to jpeg. If the parameter
	/// is missing or set to -1 the library saves the bitmap image as</para>
	/// <code>
	///	Bitmap.Save(MemoryStream, ImageFormat.Jpeg);
	///	</code>
	///	<para>If the ImageQuality parameter is 0 to 100, the library saves the bitmap image as</para>
	///	<code>
	///	EncoderParameters EncoderParameters = new EncoderParameters(1);
	/// EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
	/// Bitmap.Save(MemoryStream, GetEncoderInfo("image/jpeg"), EncoderParameters);
	///	</code>
	///	<para>Microsoft does not specify the image quality factor used in the 
	///	first method of saving. However, experimantaion and Internet comments shows that it is 75.</para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfImage
			(
			PdfDocument		Document,
			String			ImageFileName,
			Rectangle		CropRect,
			Double			Resolution = 0.0,		// pixels per inch
			Int32			ImageQuality = -1		// 0 to 100 or -1 for default save
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		ConstructorHelper(ImageFileName, null, CropRect, RectangleF.Empty, Resolution, ImageQuality);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor for image file
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="ImageFileName">Image file name</param>
	/// <param name="CropPercent">Crop rectangle (in percent of width and height)</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional)</param>
	/// <param name="ImageQuality">Image quality 0 to 100 or -1 (optional)</param>
	/// <remarks>
	/// <para>Image quality is a parameter that used by the .net framework
	/// during the compression of the image from bitmap to jpeg. If the parameter
	/// is missing or set to -1 the library saves the bitmap image as</para>
	/// <code>
	///	Bitmap.Save(MemoryStream, ImageFormat.Jpeg);
	///	</code>
	///	<para>If the ImageQuality parameter is 0 to 100, the library saves the bitmap image as</para>
	///	<code>
	///	EncoderParameters EncoderParameters = new EncoderParameters(1);
	/// EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
	/// Bitmap.Save(MemoryStream, GetEncoderInfo("image/jpeg"), EncoderParameters);
	///	</code>
	///	<para>Microsoft does not specify the image quality factor used in the 
	///	first method of saving. However, experimantaion and Internet comments shows that it is 75.</para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfImage
			(
			PdfDocument		Document,
			String			ImageFileName,
			RectangleF		CropPercent,
			Double			Resolution = 0.0,		// pixels per inch
			Int32			ImageQuality = -1		// 0 to 100 or -1 for default save
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		ConstructorHelper(ImageFileName, null, Rectangle.Empty, CropPercent, Resolution, ImageQuality);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor for image object
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="Image">Image bitmap or metafile</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional)</param>
	/// <param name="ImageQuality">Image quality 0 to 100 or -1 (optional)</param>
	/// <remarks>
	/// <para>Image quality is a parameter that used by the .net framework
	/// during the compression of the image from bitmap to jpeg. If the parameter
	/// is missing or set to -1 the library saves the bitmap image as</para>
	/// <code>
	///	Bitmap.Save(MemoryStream, ImageFormat.Jpeg);
	///	</code>
	///	<para>If the ImageQuality parameter is 0 to 100, the library saves the bitmap image as</para>
	///	<code>
	///	EncoderParameters EncoderParameters = new EncoderParameters(1);
	/// EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
	/// Bitmap.Save(MemoryStream, GetEncoderInfo("image/jpeg"), EncoderParameters);
	///	</code>
	///	<para>Microsoft does not specify the image quality factor used in the 
	///	first method of saving. However, experimantaion and Internet comments shows that it is 75.</para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfImage
			(
			PdfDocument		Document,
			Image			Image,
			Double			Resolution = 0.0,		// pixels per inch
			Int32			ImageQuality = -1		// 0 to 100 or -1 for default save
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		ConstructorHelper(null, Image, Rectangle.Empty, RectangleF.Empty, Resolution, ImageQuality);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor for image object
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="Image">Image bitmap or metafile</param>
	/// <param name="CropRect">Crop rectangle (in pixels)</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional)</param>
	/// <param name="ImageQuality">Image quality 0 to 100 or -1 (optional)</param>
	/// <remarks>
	/// <para>Image quality is a parameter that used by the .net framework
	/// during the compression of the image from bitmap to jpeg. If the parameter
	/// is missing or set to -1 the library saves the bitmap image as</para>
	/// <code>
	///	Bitmap.Save(MemoryStream, ImageFormat.Jpeg);
	///	</code>
	///	<para>If the ImageQuality parameter is 0 to 100, the library saves the bitmap image as</para>
	///	<code>
	///	EncoderParameters EncoderParameters = new EncoderParameters(1);
	/// EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
	/// Bitmap.Save(MemoryStream, GetEncoderInfo("image/jpeg"), EncoderParameters);
	///	</code>
	///	<para>Microsoft does not specify the image quality factor used in the 
	///	first method of saving. However, experimantaion and Internet comments shows that it is 75.</para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfImage
			(
			PdfDocument		Document,
			Image			Image,
			Rectangle		CropRect,
			Double			Resolution = 0.0,		// pixels per inch
			Int32			ImageQuality = -1		// 0 to 100 or -1 for default save
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		ConstructorHelper(null, Image, CropRect, RectangleF.Empty, Resolution, ImageQuality);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor for image object
	/// </summary>
	/// <param name="Document">PDF document (parent object)</param>
	/// <param name="Image">Image bitmap or metafile</param>
	/// <param name="CropPercent">Crop rectangle (in percent of width and height)</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional)</param>
	/// <param name="ImageQuality">Image quality 0 to 100 or -1 (optional)</param>
	/// <remarks>
	/// <para>Image quality is a parameter that used by the .net framework
	/// during the compression of the image from bitmap to jpeg. If the parameter
	/// is missing or set to -1 the library saves the bitmap image as</para>
	/// <code>
	///	Bitmap.Save(MemoryStream, ImageFormat.Jpeg);
	///	</code>
	///	<para>If the ImageQuality parameter is 0 to 100, the library saves the bitmap image as</para>
	///	<code>
	///	EncoderParameters EncoderParameters = new EncoderParameters(1);
	/// EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
	/// Bitmap.Save(MemoryStream, GetEncoderInfo("image/jpeg"), EncoderParameters);
	///	</code>
	///	<para>Microsoft does not specify the image quality factor used in the 
	///	first method of saving. However, experimantaion and Internet comments shows that it is 75.</para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfImage
			(
			PdfDocument		Document,
			Image			Image,
			RectangleF		CropPercent,
			Double			Resolution = 0.0,		// pixels per inch
			Int32			ImageQuality = -1		// 0 to 100 or -1 for default save
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		ConstructorHelper(null, Image, Rectangle.Empty, CropPercent, Resolution, ImageQuality);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor helper method
	////////////////////////////////////////////////////////////////////

	private void ConstructorHelper
			(
			String			ImageFileName,
			Image			Image,
			Rectangle		CropRect,
			RectangleF		CropPercent,
			Double			Resolution,			// pixels per inch
			Int32			ImageQuality
			)
		{
		// Assume we will need to dispose the Picture Bitmap
		DisposePicture = true;
		Boolean DisposeImage = false;

		// load image from file
		if(ImageFileName != null)
			{
			// test exitance
			if(!File.Exists(ImageFileName)) throw new ApplicationException("Image file " + ImageFileName + " does not exist");

			// get file length
			FileInfo FI = new FileInfo(ImageFileName);
			Int64 ImageFileLength = FI.Length;
			if(ImageFileLength >= Int32.MaxValue) throw new ApplicationException("Image file " + ImageFileName + " too long");

			// load the image file
			try
				{
				// file is metafile format
				if(ImageFileName.EndsWith(".emf", StringComparison.OrdinalIgnoreCase) || ImageFileName.EndsWith(".wmf", StringComparison.OrdinalIgnoreCase))
					Image = new Metafile(ImageFileName);

				// all other image formats
				else
					Image = new Bitmap(ImageFileName);

				// set dispose
				DisposeImage = true;
				}

			// not image file
			catch(ArgumentException)
				{
				throw new ApplicationException("Invalid image file: " + ImageFileName);
				}
			}

		// flow control flags
		Boolean Crop = !CropRect.IsEmpty || !CropPercent.IsEmpty;
		Boolean MetaFile = Image.GetType() != typeof(Bitmap);
		Boolean ChangeResolution = Resolution != 0;
		Boolean OrigNotAtZero = false;

		// image rectangle
		Rectangle ImageRect = new Rectangle(0, 0, Image.Width, Image.Height);

		// some images have origin not at top left corner
		// I fount one sample of .wmf file with origin at center of the image
		GraphicsUnit Unit = GraphicsUnit.Pixel;
		RectangleF ImageBounds = Image.GetBounds(ref Unit);
		if(ImageBounds.X != 0.0 || ImageBounds.Y != 0.0)
			{
			OrigNotAtZero = true;

			// set origin
			if(Unit == GraphicsUnit.Pixel)
				{
				ImageRect.X = (Int32) ImageBounds.X;
				ImageRect.Y = (Int32) ImageBounds.Y;
				}
			else
				{
				ImageRect.X = (Int32) (ImageBounds.X * Image.Width / ImageBounds.Width);
				ImageRect.Y = (Int32) (ImageBounds.Y * Image.Height / ImageBounds.Height);
				}
			}

		// no crop
		if(!Crop)
			{
			// get image width and height in pixels
			WidthPix = Image.Width;
			HeightPix = Image.Height;
			}

		// crop
		else
			{
			// crop rectangle is given in percent width or height
			if(!CropPercent.IsEmpty)
				{
				CropRect = new Rectangle((Int32) (0.01 * Image.Width * CropPercent.X + 0.5), (Int32) (0.01 * Image.Height * CropPercent.Y + 0.5),
					(Int32) (0.01 * Image.Width * CropPercent.Width + 0.5), (Int32) (0.01 * Image.Height * CropPercent.Height + 0.5)); 
				}

			// adjust origin
			if(OrigNotAtZero)
				{
				CropRect.X += ImageRect.X;
				CropRect.Y += ImageRect.Y;
				}

			// crop rectangle must be contained within image rectangle
			if(!ImageRect.Contains(CropRect)) throw new ApplicationException("PdfImage: Crop rectangle must be contained within image rectangle");

			// change image size to crop size
			WidthPix = CropRect.Width;
			HeightPix = CropRect.Height;

			// replace image rectangle with crop rectangle
			ImageRect = CropRect;
			}

		// destination rectangle
		Rectangle DestRect = new Rectangle(0, 0, WidthPix, HeightPix);

		// resolution pixels per inch
		Double HorizontalResolution = Image.HorizontalResolution;
		Double VerticalResolution = Image.VerticalResolution;

		// adjust resolution if it is not zero or greater than exising resolution
		if(ChangeResolution)
			{
			// image resolution
			Double ImageResolution = 0.5 * (HorizontalResolution + VerticalResolution);

			// requested resolution is less than image
			if(Resolution < ImageResolution)
				{
				// change in resolution 
				Double Factor = Resolution / ImageResolution;

				// convert to pixels based on requested resolution
				Int32 NewWidthPix = (Int32) (WidthPix * Factor + 0.5);
				Int32 NewHeightPix = (Int32) (HeightPix * Factor + 0.5);

				// new size in pixels is must be smaller than image size or cropped image size
				ChangeResolution = NewWidthPix < WidthPix && NewHeightPix < HeightPix;

				// adjust destination rectangle
				if(ChangeResolution)
					{
					// new image size in pixels
					WidthPix = NewWidthPix;
					HeightPix = NewHeightPix;

					DestRect.Width = NewWidthPix;
					DestRect.Height = NewHeightPix;

					// adjust resolution
					HorizontalResolution *= Factor;
					VerticalResolution *= Factor;
					}
				}
			else
				{
				ChangeResolution = false;
				}
			}

		// image is Bitmap (not Metafile)
		if(!MetaFile)
			{
			// no crop
			if(!Crop)
				{
				// image is bitmap, no crop, no change in resolution
				if(!ChangeResolution)
					{
					Picture = (Bitmap) Image;
					DisposePicture = DisposeImage;
					DisposeImage = false;
					}

				// image is bitmap, no crop, change to resolution
				else
					{
					// load bitmap into smaller bitmap
					Picture = new Bitmap(Image, WidthPix, HeightPix);
					}
				}

			// crop image
			else
				{
				// create bitmap
				Picture = new Bitmap(WidthPix, HeightPix);

				// create graphics object fill with white
				Graphics GR = Graphics.FromImage(Picture);

				// draw the image into the bitmap
				GR.DrawImage(Image, DestRect, ImageRect, GraphicsUnit.Pixel);

				// dispose of the graphics object
				GR.Dispose();
				}
			}

		// image is Metafile (not Bitmap)
		else
			{
			// create bitmap
			Picture = new Bitmap(WidthPix, HeightPix);

			// create graphics object fill with white
			Graphics GR = Graphics.FromImage(Picture);
			GR.Clear(Color.White);

			// draw the image into the bitmap
			GR.DrawImage(Image, DestRect, ImageRect, GraphicsUnit.Pixel);

			// dispose of the graphics object
			GR.Dispose();
			}

		// dispose image
		if(DisposeImage) Image.Dispose();

		// set resolution
		Picture.SetResolution((Single) HorizontalResolution, (Single) VerticalResolution);

		// set image quality
		if(ImageQuality < -1 || ImageQuality > 100) throw new ApplicationException("SetImageQuality argument must be -1 to 100");
		this.ImageQuality = ImageQuality;

		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// create stream length object
		ImageLengthObject = new PdfObject(Document, ObjectType.Other);
		Dictionary.AddIndirectReference("/Length", ImageLengthObject);

		// if PdfFile is open, write to output file
		WriteObjectHeaderToPdfFile();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates image size to preserve aspect ratio.
	/// </summary>
	/// <param name="InputSize">Image display area.</param>
	/// <returns>Adjusted image display area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public SizeD ImageSize
			(
			SizeD InputSize
			)
		{
		return(ImageSizePos.ImageSize(WidthPix, HeightPix, InputSize.Width, InputSize.Height));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates image size to preserve aspect ratio.
	/// </summary>
	/// <param name="Width">Image display width.</param>
	/// <param name="Height">Image display height.</param>
	/// <returns>Adjusted image display area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public SizeD ImageSize
			(
			Double	Width,
			Double	Height
			)
		{
		return(ImageSizePos.ImageSize(WidthPix, HeightPix, Width, Height));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates image size to preserve aspect ratio and sets position.
	/// </summary>
	/// <param name="InputSize">Image display area</param>
	/// <param name="Alignment">Content alignment</param>
	/// <returns>Adjusted image size and position within area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio and adjust
	/// position according to content alignment argument.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfRectangle ImageSizePosition
			(
			SizeD				InputSize,
			ContentAlignment	Alignment
			)
		{
		return(ImageSizePos.ImageArea(WidthPix, HeightPix, 0.0, 0.0,  InputSize.Width, InputSize.Height, Alignment));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates image size to preserve aspect ratio and sets position.
	/// </summary>
	/// <param name="Width">Image display width</param>
	/// <param name="Height">Image display height</param>
	/// <param name="Alignment">Content alignment</param>
	/// <returns>Adjusted image size and position within area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio and adjust
	/// position according to content alignment argument.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
    public PdfRectangle ImageSizePosition
			(
			Double				Width,
			Double				Height,
			ContentAlignment	Alignment
			)
		{
		return(ImageSizePos.ImageArea(WidthPix, HeightPix, 0.0, 0.0,  Width, Height, Alignment));
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile()
		{
		// shortcut
		PdfBinaryWriter PdfFile = Document.PdfFile;

		// add items to dictionary
		Dictionary.Add("/Subtype", "/Image");
		Dictionary.AddInteger("/Width", WidthPix);
		Dictionary.AddInteger("/Height", HeightPix);
		Dictionary.Add("/Filter", "/DCTDecode");
		Dictionary.Add("/ColorSpace", "/DeviceRGB");
		Dictionary.Add("/BitsPerComponent", "8");

		// write dictionary
		Dictionary.WriteToPdfFile();

		// output stream
		PdfFile.WriteString("stream\n");

		// save pdf file position
		Int64 streamStart = PdfFile.BaseStream.Position;

		// debug
		if(Document.Debug)
			{
			PdfFile.WriteString("*** IMAGE PLACE HOLDER ***");
			}

		// copy image file to output file
		else
			{
			// create memory stream
			MemoryStream MS = new MemoryStream();

			// image quality is -1
			if(ImageQuality >= 0)
				{
				// build EncoderParameter object for image quality
				EncoderParameters EncoderParameters = new EncoderParameters(1);
				EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);

				// save in jpeg format with specified quality
				Picture.Save(MS, GetEncoderInfo("image/jpeg"), EncoderParameters);
				}

			else
				{
				// save in jpeg format with 75 quality
				Picture.Save(MS, ImageFormat.Jpeg);
				}

			// image byte array
			Byte[] ByteContents = MS.GetBuffer();

			// close and dispose memory stream
			MS.Close();
			MS = null;

			// encryption
			if(Document.Encryption != null) ByteContents = Document.Encryption.EncryptByteArray(ObjectNumber, ByteContents);

			// write memory stream internal buffer to PDF file
			PdfFile.Write(ByteContents);

			// dispose of the image
			Dispose();
			}

		// save stream length
		ImageLengthObject.ContentsString.Append(((Int32) (PdfFile.BaseStream.Position - streamStart)).ToString());

		// output stream
		PdfFile.WriteString("\nendstream\nendobj\n");
		return;
		}

 	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

   private ImageCodecInfo GetEncoderInfo(String mimeType)
	    {
        int Index;
        ImageCodecInfo[] Encoders = ImageCodecInfo.GetImageEncoders();
        for(Index = 0; Index < Encoders.Length; ++Index)
	        {
            if(Encoders[Index].MimeType == mimeType) return(Encoders[Index]);
	        }
        throw new ApplicationException("GetEncoderInfo: image/jpeg encoder does not exist");;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Dispose unmanaged resources
	/// </summary>
	////////////////////////////////////////////////////////////////////
	public void Dispose()
		{
		// release bitmap
		if(DisposePicture && Picture != null)
			{
			Picture.Dispose();
			Picture = null;
			}

		// exit
		return;		
		}
	}
}
