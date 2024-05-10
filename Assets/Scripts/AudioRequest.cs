using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class AudioRequest
    {
        public string success { get; set; }
        public State state { get; set; }
    }

    public class State
    {
        public string job_token { get; set; }
        public string status { get; set; }
        public string maybe_extra_status_description { get; set; }
        public int attempt_count { get; set; }
        public string maybe_result_token { get; set; }
        public string maybe_public_bucket_wav_audio_path { get; set; }
        public string model_token { get; set; }
        public string tts_model_type { get; set; }
        public string title { get; set; }
        public string raw_inference_text { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}