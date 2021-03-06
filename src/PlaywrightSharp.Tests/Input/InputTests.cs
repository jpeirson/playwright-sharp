using System.IO;
using System.Threading.Tasks;
using PlaywrightSharp.Tests.BaseTests;
using Xunit;
using Xunit.Abstractions;

namespace PlaywrightSharp.Tests.Input
{
    ///<playwright-file>input.spec.js</playwright-file>
    ///<playwright-describe>input</playwright-describe>
    [Trait("Category", "chromium")]
    [Trait("Category", "firefox")]
    [Collection(TestConstants.TestFixtureBrowserCollectionName)]
    public class InputTests : PlaywrightSharpPageBaseTest
    {
        /// <inheritdoc/>
        public InputTests(ITestOutputHelper output) : base(output)
        {
        }

        ///<playwright-file>input.spec.js</playwright-file>
        ///<playwright-describe>input</playwright-describe>
        ///<playwright-it>should upload the file</playwright-it>
        [Fact]
        public async Task ShouldUploadTheFile()
        {
            await Page.GoToAsync(TestConstants.ServerUrl + "/input/fileupload.html");
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", TestConstants.FileToUpload);
            var input = await Page.QuerySelectorAsync("input");
            await input.SetInputFilesAsync(filePath);
            Assert.Equal("file-to-upload.txt", await Page.EvaluateAsync<string>("e => e.files[0].name", input));
            Assert.Equal("contents of the file", await Page.EvaluateAsync<string>(@"e => {
                var reader = new FileReader();
                var promise = new Promise(fulfill => reader.onload = fulfill);
                reader.readAsText(e.files[0]);
                return promise.then(() => reader.result);
            }", input));
        }
    }
}
