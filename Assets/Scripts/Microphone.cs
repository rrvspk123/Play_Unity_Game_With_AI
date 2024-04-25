#if UNITY_WEBGL && !UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityEngine
{
    public class Microphone
    {
        [DllImport("__Internal")]
        public static extern void Init();

        [DllImport("__Internal")]
        public static extern void QueryAudioInput();

        [DllImport("__Internal")]
        private static extern int GetNumberOfMicrophones();

        [DllImport("__Internal")]
        private static extern string GetMicrophoneDeviceName(int index);

        [DllImport("__Internal")]
        private static extern float GetMicrophoneVolume(int index);
        
        [DllImport("__Internal")]
        public static extern void StartRecording();

        [DllImport("__Internal")]
        public static extern void PauseRecording();

        [DllImport("__Internal")]
        public static extern void SyncDB();

        [DllImport("__Internal")]
        public static extern void End();

    }
}

#endif