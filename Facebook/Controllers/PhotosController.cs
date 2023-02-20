using Facebook.Data;
using Facebook.Models;
using Facebook.Repositories;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PhotosController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly DataContext _context;

        public PhotosController(UserManager<User> userManager, IWebHostEnvironment hostEnvironment,
                DataContext context)
        {
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<Photo> getMainPicture(string id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.UserId == id && p.IsMain == true);
        }

        [HttpGet("second/{id}")]
        public async Task<Photo> getSecondPicture(string id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.UserId == id && p.IsSecond == true);
        }

        [HttpGet("third/{id}")]
        public async Task<Photo> getThirdPicture(string id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.UserId == id && p.IsThird == true);
        }


        [HttpPost("upload-main-photo")]
        public async Task<ActionResult<User>> UploadMain([FromForm]PhotoDto photoDto)
        {

            var user = _userManager.Users.Include(x => x.Photos).FirstOrDefault(x => x.Id == photoDto.UserId);

            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (photoDto.fileMain != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                photoDto.fileMain.OpenReadStream().CopyTo(memoryStream);
                string photoUrl = Convert.ToBase64String(memoryStream.ToArray());

                string filename = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\users");
                var extension = Path.GetExtension(photoDto.fileMain.FileName);


                Uri domain = new Uri(Request.GetDisplayUrl());



                using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                {
                    photoDto.fileMain.CopyTo(fileStreams);
                }

                photoUrl = domain.Scheme + "://" + domain.Host + (domain.IsDefaultPort ? "" : ":" + domain.Port) + "/images/users/" + filename + extension;

                var photo = new Photo
                {
                    Url = photoUrl,
                    UserId = user.Id,
                    IsMain = true,
                };

                var oldPhoto = await _context.Photos.FirstOrDefaultAsync(p => p.UserId == photoDto.UserId && p.IsMain == true);

                if(oldPhoto != null)
                {
                    _context.Photos.Remove(oldPhoto);
                }
                
                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
                user.Photos.Add(photo);
                user.MainPhotoUrl = photoUrl;
                await _userManager.UpdateAsync(user);


            }



            return Ok(user);

        }

        [HttpPost("upload-secondary-photo")]
        public async Task<ActionResult<User>> UploadSecond([FromForm] PhotoDto photoDto)
        {

            var user = _userManager.Users.Include(x => x.Photos).FirstOrDefault(x => x.Id == photoDto.UserId);

            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (photoDto.fileSecond != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                photoDto.fileSecond.OpenReadStream().CopyTo(memoryStream);
                string photoUrl = Convert.ToBase64String(memoryStream.ToArray());

                string filename = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\users");
                var extension = Path.GetExtension(photoDto.fileSecond.FileName);


                Uri domain = new Uri(Request.GetDisplayUrl());



                using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                {
                    photoDto.fileSecond.CopyTo(fileStreams);
                }

                photoUrl = domain.Scheme + "://" + domain.Host + (domain.IsDefaultPort ? "" : ":" + domain.Port) + "/images/users/" + filename + extension;

                var photo = new Photo
                {
                    Url = photoUrl,
                    UserId = user.Id,
                    IsMain = false,
                    IsSecond = true,
                    IsThird = false
                };

                var oldPhoto = await _context.Photos.FirstOrDefaultAsync(p => p.UserId == photoDto.UserId && p.IsSecond == true);

                if(oldPhoto != null)
                {
                    _context.Photos.Remove(oldPhoto);
                }
                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
                user.Photos.Add(photo);
                await _userManager.UpdateAsync(user);


            }



            return Ok(user);

        }

        [HttpPost("upload-third-photo")]
        public async Task<ActionResult<User>> UploadThird([FromForm] PhotoDto photoDto)
        {

            var user = _userManager.Users.Include(x => x.Photos).FirstOrDefault(x => x.Id == photoDto.UserId);

            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (photoDto.fileThird != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                photoDto.fileThird.OpenReadStream().CopyTo(memoryStream);
                string photoUrl = Convert.ToBase64String(memoryStream.ToArray());

                string filename = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\users");
                var extension = Path.GetExtension(photoDto.fileThird.FileName);


                Uri domain = new Uri(Request.GetDisplayUrl());



                using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                {
                    photoDto.fileThird.CopyTo(fileStreams);
                }

                photoUrl = domain.Scheme + "://" + domain.Host + (domain.IsDefaultPort ? "" : ":" + domain.Port) + "/images/users/" + filename + extension;

                Random rand = new Random();
                var id = rand.Next(9999999);

                var photo = new Photo
                {
                    Id = id,
                    Url = photoUrl,
                    UserId = user.Id,
                    IsMain = false,
                    IsSecond = false,
                    IsThird = true
                };

                var oldPhoto = await _context.Photos.FirstOrDefaultAsync(p => p.UserId == photoDto.UserId && p.IsThird == true);

                if(oldPhoto != null)
                {
                    _context.Photos.Remove(photo);
                }

                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
                user.Photos.Add(photo);
                await _userManager.UpdateAsync(user);


            }



            return Ok(user);

        }
    }
}
