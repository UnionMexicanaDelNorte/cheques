/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfPage
//	PDF page class. An indirect PDF object.
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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace PdfFileWriter
{
/// <summary>
/// PDF page class
/// </summary>
/// <remarks>
/// PDF page class represent one page in the document.
/// </remarks>
public class PdfPage : PdfObject
	{
	internal Double		Width;
	internal Double		Height;
	internal List<PdfContents> ContentsArray;

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Default constructor
	/// </summary>
	/// <param name="Document">Parent PDF document object</param>
	/// <remarks>
	/// Page size is taken from PdfDocument
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfPage
			(
			PdfDocument Document
			) : base(Document, ObjectType.Dictionary, "/Page")
		{
		ConstructorHelper(Document.PageSize.Width, Document.PageSize.Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="Document">Parent PDF document object</param>
	/// <param name="PageSize">Paper size for this page</param>
	/// <remarks>
	/// PageSize override the default page size
	/// </remarks>
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument		Document,
			SizeD			PageSize
			) : base(Document, ObjectType.Dictionary, "/Page")
		{
		ConstructorHelper(ToPt(PageSize.Width), ToPt(PageSize.Height));
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="Document">Parent PDF document object</param>
	/// <param name="PaperType">Paper type</param>
	/// <param name="Landscape">If Lanscape is true, width and height are swapped.</param>
	/// <remarks>
	/// PaperType and orientation override the default page size.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfPage
			(
			PdfDocument		Document,
			PaperType		PaperType,
			Boolean			Landscape
			) : base(Document, ObjectType.Dictionary, "/Page")
		{
		// get standard paper size
		Double Width = PdfDocument.PaperTypeSize[(Int32) PaperType].Width;
		Double Height = PdfDocument.PaperTypeSize[(Int32) PaperType].Height;

		// for landscape swap width and height
		if(Landscape)
			{
			Double Temp = Width;
			Width = Height;
			Height = Temp;
			}

		// exit
		ConstructorHelper(Width, Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="Document">Parent PDF document object</param>
	/// <param name="Width">Page width</param>
	/// <param name="Height">Page height</param>
	/// <remarks>
	/// Width and Height override the default page size
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfPage
			(
			PdfDocument		Document,
			Double			Width,
			Double			Height
			) : base(Document, ObjectType.Dictionary, "/Page")
		{
		ConstructorHelper(ToPt(Width), ToPt(Height));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor common method
	////////////////////////////////////////////////////////////////////

	private void ConstructorHelper
			(
			Double	Width,
			Double	Height
			)
		{
		// save page width and height in user units
		this.Width = Width / ScaleFactor;
		this.Height = Height / ScaleFactor;

		// add page to parent array of pages
		Document.PageArray.Add(this);

		// link page to parent
		Dictionary.AddIndirectReference("/Parent", Document.PagesObject);

		// add page size in points
		Dictionary.AddFormat("/MediaBox", "[0 0 {0} {1}]", Round(Width), Round(Height));

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Add existing contents to page
	/// </summary>
	/// <param name="Contents">Contents object</param>
	////////////////////////////////////////////////////////////////////
	public void AddContents
			(
			PdfContents	Contents
			)
		{
		// set page contents flag
		Contents.PageContents = true;
 
		// add content to content array
		if(ContentsArray == null) ContentsArray = new List<PdfContents>();
		ContentsArray.Add(Contents);

		// exit
		return;
		}

	/// <summary>
	/// Gets the current contents of this page
	/// </summary>
	/// <returns>Page's current contents</returns>
	public PdfContents GetCurrentContents()
		{
		return((ContentsArray == null || ContentsArray.Count == 0) ? null : ContentsArray[ContentsArray.Count - 1]);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Add weblink to this page
	/// </summary>
	/// <param name="LeftPos">Left position of weblink area</param>
	/// <param name="BottomPos">Bottom position of weblink area</param>
	/// <param name="RightPos">Right position of weblink area</param>
	/// <param name="TopPos">Top position of weblink area</param>
	/// <param name="WebLink">Hyperlink string</param>
	/// <remarks>
	/// <para>
	///	The four position arguments are in relation to the
	///	bottom left corner of the paper.
	///	If web link is empty, ignore this call.
	/// </para>
	/// <para>
	/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#WeblinkSupport">2.7 Web Link Support</a>
	/// </para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public void AddWebLink
			(
			Double		LeftPos,
			Double		BottomPos,
			Double		RightPos,
			Double		TopPos,
			String		WebLink
			)
		{
		AddWebLink(new PdfRectangle(LeftPos, BottomPos, RightPos, TopPos), WebLink);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Add weblink to this page
	/// </summary>
	/// <param name="WeblinkArea">Weblink area</param>
	/// <param name="WebLinkStr">Hyperlink string</param>
	/// <remarks>
	/// <para>
	///	The four position arguments are in relation to the
	///	bottom left corner of the paper.
	///	If web link is empty, ignore this call.
	/// </para>
	/// <para>
	/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#WeblinkSupport">2.7 Web Link Support</a>
	/// </para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public void AddWebLink
			(
			PdfRectangle	WeblinkArea,
			String			WebLinkStr
			)
		{
		// if web link is empty, ignore this call
		if(String.IsNullOrWhiteSpace(WebLinkStr)) return;

		// create weblink action or reuse duplicate
		PdfWebLink WebLinkAction = PdfWebLink.AddWebLink(Document, WebLinkStr);

		// create PdfObject for annotation
		PdfAnnotation Annotation = new PdfAnnotation(this, WeblinkArea, WebLinkAction);

		// exit
		return;
		}

	/// <summary>
	/// Add rendering screen action to page
	/// </summary>
	/// <param name="Rect">Annotation rectangle</param>
	/// <param name="FileName">Media file name</param>
	/// <param name="WidthPix">Video width in pixels</param>
	/// <param name="HeightPix">Video height in pixels</param>
	/// <param name="MimeType">Media file Mime type</param>
	public void AddScreenAction
			(
			PdfRectangle	Rect,
			String			FileName,
			Int32			WidthPix = 0,
			Int32			HeightPix = 0,
			String			MimeType = null
			)
		{
		// create embedded media file
		PdfEmbeddedFile MediaFile = new PdfEmbeddedFile(Document, FileName);

		// Section 8.5 page 669 table 8.64
		PdfDisplayMedia ScreenAction = new PdfDisplayMedia(MediaFile, MimeType);

		// create PdfObject for annotation
		PdfAnnotation Annotation = new PdfAnnotation(this, Rect, ScreenAction);

//		Annotation.DisplayBorder(0.1);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile()
		{
		// we have at least one contents object
		if(ContentsArray != null)
			{
			// page has one contents object
			if(ContentsArray.Count == 1)
				{
				Dictionary.AddFormat("/Contents", "[{0} 0 R]", ContentsArray[0].ObjectNumber);
				Dictionary.Add("/Resources", BuildResourcesDictionary(ContentsArray[0].ResObjects, true));
				}

			// page is made of multiple contents
			else
				{
				// contents dictionary entry
				StringBuilder ContentsStr = new StringBuilder("[");

				// build contents dictionary entry
				foreach(PdfContents Contents in ContentsArray) ContentsStr.AppendFormat("{0} 0 R ", Contents.ObjectNumber);

				// add terminating bracket
				ContentsStr.Length--;
				ContentsStr.Append(']');
				Dictionary.Add("/Contents", ContentsStr.ToString());

				// resources array of all contents objects
				List<PdfObject> ResObjects = new List<PdfObject>();

				// loop for all contents objects
				foreach(PdfContents Contents in ContentsArray)
					{
					// make sure we have resources
					if(Contents.ResObjects != null)
						{
						// loop for resources within this contents object
						foreach(PdfObject ResObject in Contents.ResObjects)
							{
							// check if we have it already
							Int32 Ptr = ResObjects.BinarySearch(ResObject);
							if(Ptr < 0) ResObjects.Insert(~Ptr, ResObject);
							}
						}
					}

				// save to dictionary
				Dictionary.Add("/Resources", BuildResourcesDictionary(ResObjects, true));
				}
			}

		// call PdfObject routine
		base.WriteObjectToPdfFile();

		// exit
		return;
		}
	}
}
