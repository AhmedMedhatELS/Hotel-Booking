﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Utility
{
    public enum ImageLocation
    {
        Users,
        Hotels,
        Rooms
    }
    public static class ImageManger
    {
        private static readonly Dictionary<ImageLocation, string> location = new()
        {
            {ImageLocation.Users, "wwwroot/images/users"},
            {ImageLocation.Hotels, "wwwroot/images/Hotels"},
            {ImageLocation.Rooms, "wwwroot/images/Rooms"}
        };

        public static string? SaveImage(IFormFile ImageFile,ImageLocation imageLocation)
        {
            string? ImageName = null;

            if (ImageFile != null)
            {
                // Generate a unique file name without changing the extension
                ImageName  = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";

                // Define the path to save the image
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), location[imageLocation], ImageName);

                //copy the image to the desired location
                using var fileStream = new FileStream(imagePath, FileMode.Create);
                ImageFile.CopyTo(fileStream);
            }

            return ImageName;
        }
        public static List<string> SaveImageList(List<IFormFile> ImageFiles, ImageLocation imageLocation)
        {
            List<string> ImageNames = [];

            foreach (IFormFile ImageFile in ImageFiles)
            {
                if (ImageFile != null)
                {
                    // Generate a unique file name without changing the extension
                    var currentImageName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";

                    // Define the path to save the image
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), location[imageLocation], currentImageName);

                    //copy the image to the desired location
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    ImageFile.CopyTo(fileStream);

                    //Add Name To The List
                    if(currentImageName != null)
                        ImageNames.Add(currentImageName);
                }
            }
            return ImageNames;
        }
        public static void DeleteImage(string ImageName, ImageLocation imageLocation)
        {
            if (!string.IsNullOrEmpty(ImageName))
            {
                var pathimg = Path.Combine(Directory.GetCurrentDirectory(), location[imageLocation], ImageName);
                FileInfo file = new(pathimg);
                if (file.Exists)
                    file.Delete();                
            }
        }
        public static void DeleteImageList(List<string> ImageNames, ImageLocation imageLocation)
        {
            foreach (string ImageName in ImageNames)
            {
                if (!string.IsNullOrEmpty(ImageName))
                {
                    var pathimg = Path.Combine(Directory.GetCurrentDirectory(), location[imageLocation], ImageName);
                    FileInfo file = new(pathimg);
                    if (file.Exists)
                        file.Delete();
                }
            }
        }
    }
}