using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Threading;
using System.IO;


namespace PROG6221_Part1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Play the voice greeting and display the ASCII art
            PlayVoice();
            DisplayAscii();

            // Create an instance of the ChatBot class and start the conversation
            ChatBot chatBot = new ChatBot();
            chatBot.Conversation();

        }
        // Method to play a welcome audio message
        static void PlayVoice()
        {
            try
            {
                // Load and play the sound file from the Assets folder
                using (SoundPlayer player = new SoundPlayer("Assets/greeting.wav"))
                {
                    // Load the sound file  
                    player.Load();
                    // Plays audio and waits until it finishes 
                    player.PlaySync();   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with playing the sound: " + ex.Message);
            }
        }
        // Method to read and display ASCII art from a text file
        static void DisplayAscii()
        {
            try
            {
               // Read the ASCII art text from the file
               String asciiArt = File.ReadAllText("Assets/asciiArt.txt");
               Console.ForegroundColor = ConsoleColor.Red;
               Console.WriteLine(asciiArt);
               Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with displaying the ASCII art: " + ex.Message);
            }
        }
        
    }

}
