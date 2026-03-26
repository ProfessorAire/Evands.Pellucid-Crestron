// UploadGitHubAssets.cake - Upload packaged zip and NuGet package to GitHub release

using System.Net.Http.Headers;
using System.Net.Http;
#load "./Environment.cake"
var artifactsDir = GetArtifactsPath();

Task("Default")
    .Does(async () =>
{
    var githubToken = Argument("githubToken", EnvironmentVariable("GITHUB_TOKEN"));
    if (string.IsNullOrWhiteSpace(githubToken)) throw new InvalidOperationException("GitHub token not provided. Use the --githubToken argument or set GITHUB_TOKEN env var.");

    var uploadTemplate = GetEnvironmentVariable("UPLOAD_URL");
    if (string.IsNullOrWhiteSpace(uploadTemplate))
    {
         throw new InvalidOperationException("UPLOAD_URL not found. Run CreateGitHubRelease first.");
    }

    // Upload zip asset
    var zips = GetFiles(System.IO.Path.Combine(artifactsDir, "Evands.Pellucid-Crestron-v*.zip"));
    if (!zips.Any()) throw new InvalidOperationException("No packaged zip artifact found in artifacts directory. Run PackageAssets task first.");
    
    foreach (var packagePath in zips)
    {
        var fileName = System.IO.Path.GetFileName(packagePath.FullPath);
        await UploadAsset(githubToken, uploadTemplate, packagePath.FullPath, fileName, "application/zip");
    }

    // Upload NuGet package
    var nupkgs = GetFiles(System.IO.Path.Combine(artifactsDir, "*.nupkg"));
    foreach (var nupkg in nupkgs)
    {
        var fileName = System.IO.Path.GetFileName(nupkg.FullPath);
        await UploadAsset(githubToken, uploadTemplate, nupkg.FullPath, fileName, "application/octet-stream");
    }
});

async System.Threading.Tasks.Task UploadAsset(string githubToken, string uploadTemplate, string filePath, string fileName, string contentType)
{
    var uploadUrl = uploadTemplate;
    var idx = uploadUrl.IndexOf('{');
    if (idx >= 0) uploadUrl = uploadUrl.Substring(0, idx);
    var finalUrl = $"{uploadUrl}?name={System.Net.WebUtility.UrlEncode(fileName)}";

    using var http = new System.Net.Http.HttpClient();
    http.DefaultRequestHeaders.UserAgent.ParseAdd("Evands.Pellucid-Cake-Upload");
    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", githubToken);

    using var fs = System.IO.File.OpenRead(filePath);
    var content = new StreamContent(fs);
    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
    var resp = await http.PostAsync(finalUrl, content);
    var respBody = await resp.Content.ReadAsStringAsync();
    if (!resp.IsSuccessStatusCode) throw new InvalidOperationException($"Upload of {fileName} failed: {resp.StatusCode}\n{respBody}");
    Information($"Uploaded asset {fileName} to release.");
}

RunTarget(Argument("target", "Default"));
