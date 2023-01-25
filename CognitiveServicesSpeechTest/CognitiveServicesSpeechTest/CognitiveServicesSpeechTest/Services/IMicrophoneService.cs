using System;
using System.Threading.Tasks;

namespace CognitiveServicesSpeechTest.Services
{
	public interface IMicrophoneService
	{
        Task<bool> GetPermissionsAsync();
        void OnRequestPermissionsResult(bool isGranted);
    }
}

