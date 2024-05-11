using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class AudioRequest
    {
        public string Success { get; set; }
        public string Inference_job_token { get; set; }
        public State State { get; set; }
    }

    public class State
    {
        public string Job_token { get; set; }
        public string Status { get; set; }
        public string Maybe_extra_status_description { get; set; }
        public int Attempt_count { get; set; }
        public string Maybe_result_token { get; set; }
        public string Maybe_public_bucket_wav_audio_path { get; set; }
        public string Model_token { get; set; }
        public string Tts_model_type { get; set; }
        public string Title { get; set; }
        public string Raw_inference_text { get; set; }
        public string Created_at { get; set; }
        public string Updated_at { get; set; }
    }
}