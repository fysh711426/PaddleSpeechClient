using PaddleSpeechClient;
using System;
using System.Threading.Tasks;

namespace Example
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var executor = new TTSClientExecutor(
                input: "今天天气十分不错。", output: @"output.wav");
            await executor.Execute();
        }
    }
}
