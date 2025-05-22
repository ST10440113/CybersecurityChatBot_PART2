using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CybersecurityChatBot_PART2
{
    class Program
    {
        static List<string> chatHistory = new List<string>();
        static SpeechSynthesizer synth = new SpeechSynthesizer
        {
            Volume = 100,
            Rate = 0
        };
        static Random random = new Random();

        static void Main(string[] args)
        {
            //play greeting audio  
            PlayGreetingAudio("Recording.wav");

            //set up the console window  
            Console.Title = "Cybersecurity ChatBot";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(new string('=', Console.WindowWidth));   //top border  

            //ASCII art centered  
            string[] asciiArtLines = new string[] {



"  _____     _                               _ _          _____     _      ",
" |     |_ _| |_ ___ ___ ___ ___ ___ _ _ ___|_| |_ _ _   | __  |___| |_    ",
" |   --| | | . | -_|  _|_ -| -_|  _| | |  _| |  _| | |  | __ -| . |  _|   ",
" |_____|_  |___|___|_| |___|___|___|___|_| |_|_| |_  |  |_____|___|_|     ",
"       |___|                                     |___|                    ",

               };

            int consoleWidth = Console.WindowWidth;
            foreach (string line in asciiArtLines)
            {
                int padding = (consoleWidth - line.Length) / 2;
                Console.WriteLine(new string(' ', Math.Max(0, padding)) + line);
            }

            Console.WriteLine(new string('=', Console.WindowWidth));   //bottom border
            TypingEffect("Welcome to the Cybersecurity Awareness ChatBot!\n");

            //Ask for the users name
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("What's your name: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string userName = Console.ReadLine();

           
            Console.ForegroundColor = ConsoleColor.White;
            RespondWithSpeech($"Hi {userName} , I'm here to help you stay safe online!\n");

            //Display security tip of the day
            DisplayTipOfTheDay();


            //Display topics that the user can ask about
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('-', 50));

            //Start the chat loop
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"\n{userName}: ");
                string userInput = Console.ReadLine().ToLower().Trim();

                //Checks for an empty input
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    RespondWithSpeech("Please enter a valid input.");
                    continue;
                }
                //Checks for exit command
                if (userInput == "exit" || userInput == "quit")
                {
                   
                    Console.ForegroundColor = ConsoleColor.Red;
                    RespondWithSpeech(" Stay safe and think before you click online. Goodbye!");
                    break;
                }
                HandleUserQuery(userInput, userName);

            }

            //Save chat history when exiting
            SaveChatHistory();

        }
        static void PlayGreetingAudio(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                //Check if the file exists
                if (File.Exists(fullPath))
                {
                    SoundPlayer audio = new SoundPlayer(fullPath);
                    audio.PlaySync();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: the file '{filePath}' was not found");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error playing audio: {ex.Message}");
            }
        }


        static void HandleUserQuery(string input, string userName)
        {
            Dictionary<string, string> responses = new Dictionary<string, string>
            {
                { "cybersecurity", "Cybersecurity is the practice of protecting systems, networks, and programs from digital attacks." },
                { "phishing", "Phishing is a type of cyber attack where attackers impersonate legitimate organizations to steal sensitive information." },
                { "malware", "Malware is malicious software designed to harm or exploit any programmable device or network." },
                { "ransomware", "Ransomware is a type of malware that encrypts files and demands payment for the decryption key." },
                { "firewall", "A firewall is a network security system that monitors and controls incoming and outgoing network traffic." },
                { "antivirus", "Antivirus software is designed to detect and destroy computer viruses and other malicious software." },
                { "password", "A strong password should be at least 12 characters long and include a mix of letters, numbers, and symbols." },
                { "social engineering", "Social engineering is the psychological manipulation of people into performing actions or divulging confidential information." },
                { "How are you?"," I'm good thanks. How can I assist you today?" },
                { "What's your purpose?","I'm here to help you stay safe online by giving cybersecurity advice" },
                { "What can I ask you about?", "You can ask me about cybersecurity, phishing,malware,ransomeware,firewall,antiviruses, passwords ,and social engineering" },
                { "help", "You can ask about cybersecurity, phishing,malware,ransomeware,firewall,antiviruses, passwords ,and social engineering" },
                { "What is your name?","I'm the Cybersecurity ChatBot, here to assist you with your cybersecurity questions." },
                { "Tell me a joke", "Why did the computer go to therapy? Because it had too many bytes!" },
                { "Tell me a fun fact", "Did you know that the first computer virus was created in 1983?" },


            };

            


    Dictionary<string, List<string>> keywordGroups = new Dictionary<string, List<string>>
    {
    { "cybersecurity", new List<string> { "cyber security", "online safety", "information security", "cyber attack", "cyber threats" } },
    { "phishing", new List<string> { "phishing emails", "phishing", "fake emails", "email scam", "spear phishing", "phishing attack" } },
    { "malware", new List<string> { "malicious software", "virus", "trojan", "worm", "spyware", "adware", "malware infection" } },
    { "ransomware", new List<string> { "ransomware attack", "encrypted files", "pay ransom", "ransom demand" } },
    { "firewall", new List<string> { "network firewall", "firewall protection", "block traffic", "security firewall" } },
    { "antivirus", new List<string> { "antivirus software", "virus protection", "antimalware", "scan for viruses" } },
    { "password", new List<string> { "strong password", "password security", "password manager", "password protection" } },
    { "social engineering", new List<string> { "social engineering attack", "manipulation", "pretexting", "baiting", "impersonation" } }
    };


            Dictionary<string, string> followUpQuestions = new Dictionary<string, string>
            {
                { "Tell me more about cybersecurity", "Would you like to know more about specific cybersecurity threats or best practices?" },
                { "Tell me more about phishing", "Are you interested in learning how to recognize phishing attempts?" },
                { "Tell me more about malware", "Would you like tips on how to protect your device from malware?" },
                { "Tell me more about ransomware", "Do you want to know how to recover from a ransomware attack?" },
                { "Tell me more about firewall", "Would you like to learn how to configure a firewall for better security?" },
                { "Tell me more about antivirus", "Are you looking for recommendations on antivirus software?" },
                { "Tell me more about password", "Would you like tips on creating strong passwords?" },
                { "Tell me more about social engineering", "Do you want to learn how to avoid falling victim to social engineering attacks?" }
            };
             


            bool foundResponse = false;

            chatHistory.Add($"{userName}: {input}");

            foreach (var entry in responses)
            {
                if (input.Contains(entry.Key))
                {
                    RespondWithSpeech(entry.Value);
                    foundResponse = true;
                    break;
                }
            }
            if (!foundResponse)
            {
                foreach (var group in keywordGroups)
                {
                    foreach (var synonym in group.Value)
                    {
                        if (input.Contains(synonym))
                        {
                            RespondWithSpeech(responses[group.Key]);
                            foundResponse = true;
                            break;
                        }
                    }
                    if (foundResponse) break;

                }
            }
            if (!foundResponse)
            {
                RespondWithSpeech("I'm sorry, I don't have information on that topic. Enter the word 'help' to see what you can ask about. ");
            }
        }
        static void DisplayTipOfTheDay()
        {
            string[] tips = new string[]
            {
                "Always use strong and unique passwords for each of your accounts.",
                "Enable two-factor authentication wherever possible.",
                "Be cautious of unsolicited emails and messages asking for personal information.",
                "Keep your software and operating system up to date.",
                "Regularly back up your important data to an external drive or cloud storage."
            };
            int tipIndex = random.Next(tips.Length);
            LoadingEffect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\nSecurity Tip of the Day: {tips[tipIndex]}");
        }

        static void TypingEffect(string message, int delay = 30)
        {

            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        static void LoadingEffect()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Chatbot");
            for (int i = 0; i < 3; i++)
            {
               
                Thread.Sleep(400);
                Console.Write(".");

            }
            
            Console.WriteLine();
        }

        static void SaveChatHistory()
        {
            string filePath = "chat_history.txt";

            File.WriteAllLines(filePath, chatHistory);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Chat history saved successfully to {filePath}");
        }


        static void RespondWithSpeech(string response)
        {
            LoadingEffect();
            Console.ForegroundColor = ConsoleColor.Blue;
            TypingEffect($"Chatbot: {response}\n");


            try
            {
                synth.SpeakAsync(response);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
            }

            chatHistory.Add($"Chatbot: {response}");
        }



    }
}
