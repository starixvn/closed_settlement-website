<%@ WebHandler Language="C#" Class="NewsImageUpload" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;


public class NewsImageUpload : IHttpHandler, IRequiresSessionState
{

    private const String SUCCESS_MESSAGE = "Success";
    private const String INVALID_FILE_EXTENSION_MESSAGE = "Sorry, You choose invalid file extension. Please choose an image file.";
    private const String INVALID_FILE_SIZE_MESSAGE = "Your upload file Size must be <= 1 MB";
    private const String UPLOAD_PERMISSION_DENY_MESSAGE = "Your do not have permission to upload the file";
    private const String UPLOAD_FOLDER = "../../Others/Images/";
    private const String VALID_EXTENSION = "|bmp|dib|gif|jpg|jpeg|jpe|jfif|tif|tiff|png|";
    private const int MAX_FILE_SIZE = 1024000;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";

        bool isError = false;
        String imageUrl = "";
        String errorMessage = SUCCESS_MESSAGE;

        bool hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
        if (hostPermission)
        {
            try
            {
                if (context.Request.ContentLength < MAX_FILE_SIZE)
                {
                    string newFileName = context.Request.Files[0].FileName;
                    string ext = string.Empty;
                    if (!string.IsNullOrEmpty(newFileName))
                    {
                        if (IsValidExtension(newFileName, ref ext))
                        {
                            string strFileName = System.Guid.NewGuid().ToString() + "." + ext;
                            try
                            {
                                var whatnews_upload_dir = System.Configuration.ConfigurationManager.AppSettings["whatnews_upload_dir"];
                                string strLocation = context.Server.MapPath(whatnews_upload_dir) + strFileName;
                                context.Request.Files[0].SaveAs(strLocation);
                            }
                            catch (System.IO.IOException exception)
                            {
                                errorMessage = exception.Message;
                            }
                            //  Response Data 
                            isError = false;
                            var whatnews_upload_url = System.Configuration.ConfigurationManager.AppSettings["whatnews_upload_url"];
                            imageUrl = String.Format(whatnews_upload_url,  strFileName);
                            errorMessage = SUCCESS_MESSAGE;
                        }
                        else
                        {
                            isError = true;
                            errorMessage = INVALID_FILE_EXTENSION_MESSAGE;
                        }
                    }
                    else
                    {
                        isError = true;
                        errorMessage = INVALID_FILE_EXTENSION_MESSAGE;
                    }
                }
                else
                {
                    isError = true;
                    errorMessage = INVALID_FILE_SIZE_MESSAGE;
                }
            }
            
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               LogExtensions.ExceptionHandler(ex);
            }
        }
        else
        {
            errorMessage = UPLOAD_PERMISSION_DENY_MESSAGE;
        }

        String htmlData = String.Format("{0}|{1}|{2}", isError, errorMessage, imageUrl);
        context.Response.Write(htmlData);
    }


    #region Utility
    private bool IsValidExtension(string fileName, ref string ext)
    {
        string[] arr = fileName.Split('\\');
        string[] arr1 = arr[arr.Length - 1].Split('.');
        ext = arr1[arr1.Length - 1].ToLower();
        return VALID_EXTENSION.IndexOf("|" + ext + "|") >= 0 ? true : false;
    }

    #endregion

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
