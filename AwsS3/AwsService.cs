using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Common.Interfaces;
using AwsS3.Common;
using Microsoft.Extensions.Options;

namespace AwsS3;

public class AwsService : IAwsService
{
    private readonly IAmazonS3 _s3;
    private readonly AwsCfg _cfg;

    public AwsService(IOptions<AwsCfg> opts)
    {
        _cfg = opts.Value;
        _s3 = new AmazonS3Client(new BasicAWSCredentials(_cfg.AccessToken, _cfg.SecretKey),
            RegionEndpoint.GetBySystemName(_cfg.Region));
    }

    public async Task DeleteFile(string fileName)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _cfg.BucketName,
                Key = fileName,
            };

            var response = await _s3.DeleteObjectAsync(request);
            if (response.HttpStatusCode != HttpStatusCode.NoContent)
            { 
                throw new Exception($"Response status code is {response.HttpStatusCode}");
            }
        }
        catch (AmazonClientException e)
        {
            throw new Exception($"Error encountered ***. Message: {e.Message} when deleting an object");
        }
        catch (Exception e)
        {
            throw new Exception($"Unknown encountered ***. Message: {e.Message} when deleting an object");
        }
    }

    public async Task UploadFile(string fileName, string path, string contentType)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _cfg.BucketName,
                Key = fileName,
                FilePath = path,
                ContentType = contentType
            };
            
            var response = await _s3.PutObjectAsync(request);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            { 
                throw new Exception($"Response status code is {response.HttpStatusCode}");
            }
        }
        catch (AmazonClientException e)
        {
            throw new Exception($"Error encountered ***. Message: {e.Message} when uploading an object");
        }
        catch (Exception e)
        {
            throw new Exception($"Unknown encountered ***. Message: {e.Message} when uploading an object");
        }
    }

    public async Task UploadFile(string fileName, Stream stream, string contentType)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _cfg.BucketName,
                Key = fileName,
                InputStream = stream,
                ContentType = contentType
            };
            
            var response = await _s3.PutObjectAsync(request);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            { 
                throw new Exception($"Response status code is {response.HttpStatusCode}");
            }
        }
        catch (AmazonClientException e)
        {
            throw new Exception($"Error encountered ***. Message: {e.Message} when uploading an object");
        }
        catch (Exception e)
        {
            throw new Exception($"Unknown encountered ***. Message: {e.Message} when uploading an object");
        }
    }
}
