using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace aspnet_core_upload_files_to_aws_s3.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly AmazonS3Client _client;
        private readonly IOptions<AWSSettings> _awsSettings;

        public FilesController(IOptions<AWSSettings> awsSettings)
        {
            _awsSettings = awsSettings;
            _client = new AmazonS3Client(_awsSettings.Value.AccessKeyId, _awsSettings.Value.SecretKey, RegionEndpoint.APSoutheast2);
        }

        [HttpGet]
        public IActionResult GetFiles()
        {
            return Ok(new string[] { "file1", "file2" });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    using (var newMemoryStream = new MemoryStream())
                    {
                        file.CopyTo(newMemoryStream);

                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream,
                            Key = Guid.NewGuid().ToString(),
                            BucketName = _awsSettings.Value.BucketName,
                            CannedACL = S3CannedACL.PublicRead
                        };

                        var transferUtility = new TransferUtility(_client);
                        await transferUtility.UploadAsync(uploadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok();
        }
    }
}
