using System;
using CognitiveServicesSpeechTest.Services;
using Foundation;

namespace CognitiveServicesSpeechTest.iOS.Services
{
	public class AssetService : IAssetService
    {
        public string GetAssetPath(string filename)
        {
            var filePath = NSBundle.MainBundle.PathForResource(filename, "");
            return filePath;
        }
    }
}

