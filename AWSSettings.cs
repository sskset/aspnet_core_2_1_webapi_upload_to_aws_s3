using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_upload_files_to_aws_s3
{
    public class AWSSettings
    {
        public string AccessKeyId { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
    }
}
