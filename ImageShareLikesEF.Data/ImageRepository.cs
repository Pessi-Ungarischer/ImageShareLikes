using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ImageShareLikes.Data
{
    public class ImageRepository
    {
        private string _connectionString;

        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<Image> GetImages()
        {
            using ImageDbContext context = new(_connectionString);
            return context.Images.OrderByDescending(i => i.Date).ToList();
        }


        public Image GetImageById(int imageId)
        {
            using ImageDbContext context = new(_connectionString);
            return context.Images.FirstOrDefault(image => image.Id == imageId);
        }


        public void Update(Image image)
        {
            using ImageDbContext context = new ImageDbContext(_connectionString);
            context.Images.Attach(image);
            context.Entry(image).State = EntityState.Modified;
            context.SaveChanges();
        }


        public void Upload(Image image)
        {
            using ImageDbContext context = new(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }


    }
}
