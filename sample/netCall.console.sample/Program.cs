using netAudio.console.sample;
using netAudio.core;
using System;

namespace netAudio.console.sample
{
    class Program
    {
        static void Main()
        {
            //Here we create our Source and Target wich represent the end of the Audio Pipe
            var source = new DemoSource("Let me be your Demo");
            var target = new ConsoleTarget();

            //Processors allow us to manipulate our data before reach the Target
            var upProcessor = new ToUpperProcessor();
            var reverseProcessor = new ReverseProcessor();

            //Create your pipe and stick as much Processors into it as needed
            var pipe = new AudioPipe(source, target, upProcessor, reverseProcessor);

            //You should open your target before the Source to ensure to not loose any incomming data
            target.Open();
            source.Open();

            pipe.Open();
            //right here lies a lot of time while you stream audio as you please
            pipe.Close();

            //For the same reason you should close your Source first
            source.Close();
            target.Close();


            //sources and targets need to disposed by hand, since you may wanna use them multible times
            //(broadcasting or else)
            source.Dispose();
            target.Dispose();

            //same goes for the processors
            //(package integrated processors are designed stateless and can be used multible times)
            upProcessor.Dispose();
            reverseProcessor.Dispose();
           
            Console.ReadLine();
        }
    }
}
