namespace BarberSalon.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _client;


        public SupabaseService(IConfiguration config)
        {
            _client = new Supabase.Client(
                config["Supabase:Url"],
                config["Supabase:Key"]
            );
        }


        public async Task<string> UploadImage(IFormFile file)
        {
            await _client.InitializeAsync();

            var bucket = _client.Storage
                .From("services");


            var fileName = Guid.NewGuid()
                + Path.GetExtension(file.FileName);


            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);


            byte[] fileBytes = memoryStream.ToArray();


            await bucket.Upload(
                fileBytes,
                fileName
            );


            return bucket.GetPublicUrl(fileName);
        }
    }
}
