using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaddleSpeechClient
{
    public class TTSClientExecutor
    {
        protected string _serverIp = "";
        protected int _port = 0;
        protected string _input = "";
        protected int _spkId = 0;
        protected double _speed = 0.0;
        protected double _volume = 0.0;
        protected int _sampleRate = 0;
        protected string _output = "";

        /// <summary>
        /// Visit tts service.
        /// </summary>
        /// <param name="serverIp">Server ip.</param>
        /// <param name="port">Server port.</param>
        /// <param name="input">Text to be synthesized.</param>
        /// <param name="spkId">Speaker id.</param>
        /// <param name="speed">Audio speed, the value should be set between 0 and 3.</param>
        /// <param name="volume">Audio volume, the value should be set between 0 and 3.</param>
        /// <param name="sampleRate">Sampling rate, the default is the same as the model.</param>
        /// <param name="output">Synthesized audio file.</param>
        public TTSClientExecutor(
            string serverIp = "127.0.0.1", int port = 8090,
            string input = "", int spkId = 0,
            double speed = 1.0, double volume = 1.0,
            int sampleRate = 0, string output = "")
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("input cannot be empty");

            if (string.IsNullOrWhiteSpace(output))
                throw new Exception("output cannot be empty");

            _serverIp = serverIp;
            _port = port;
            _input = input;
            _spkId = spkId;
            _speed = speed;
            _volume = volume;
            _sampleRate = sampleRate;
            _output = output;
        }

        /// <summary>
        /// API to call an executor.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> Execute(CancellationToken token = default)
        {
            var clinet = Http.Client;

            var url = $"http://{_serverIp}:{_port}/paddlespeech/tts";

            var data = JsonConvert.SerializeObject(new
            {
                text = _input,
                spk_id = _spkId,
                speed = _speed,
                volume = _volume,
                sample_rate = _sampleRate,
                save_path = _output
            });

            using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
            {
                using (var response = await clinet.PostAsync(url, content, token))
                {
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
