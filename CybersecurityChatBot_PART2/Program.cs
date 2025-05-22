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

//cybersecurity follow up questions
{ "What are the most common cyber threats?", "The most common cyber threats include phishing, malware, ransomware, social engineering, and denial-of-service attacks." },
{ "How can I protect my personal information online?", "Protect your personal information by using strong passwords, enabling two-factor authentication, avoiding suspicious links, and keeping your software updated." },
{ "What is the importance of cybersecurity in daily life?", "Cybersecurity is important to protect your personal data, financial information, and privacy from cybercriminals and online threats." },

//phishing follow up questions
{ "How can I recognize a phishing email?", "Phishing emails often have urgent messages, suspicious links, spelling errors, and ask for sensitive information. Always verify the sender's address." },
{ "What should I do if I clicked a phishing link?", "If you clicked a phishing link, disconnect from the internet, run a security scan, change your passwords, and monitor your accounts for suspicious activity." },
{ "Are there tools to help prevent phishing attacks?", "Yes, use email filters, browser security extensions, and keep your antivirus software updated to help prevent phishing attacks." },

//malware follow up questions 
{ "What are the signs of a malware infection?", "Signs of malware infection include slow performance, unexpected pop-ups, new toolbars, and programs opening or closing automatically." },
{ "How do I remove malware from my computer?", "To remove malware, run a full scan with updated antivirus software and follow its removal instructions. In severe cases, seek professional help." },
{ "What types of malware exist?", "Common types of malware include viruses, worms, trojans, ransomware, spyware, and adware." },

//ransomware follow up questions
{ "What should I do if my files are encrypted by ransomware?", "Disconnect from the network, do not pay the ransom, and seek professional help. Restore files from backups if available." },
{ "How can I prevent ransomware attacks?", "Prevent ransomware by keeping backups, updating software, avoiding suspicious links, and using strong security tools." },
{ "Should I pay the ransom if infected?", "It is not recommended to pay the ransom, as it does not guarantee file recovery and encourages further attacks." },

//firewall follow up questions
{ "What is the difference between hardware and software firewalls?", "Hardware firewalls protect your entire network, while software firewalls protect individual devices. Both are important for layered security." },
{ "How do I configure a firewall?", "Configure a firewall by following the manufacturer's instructions, blocking unnecessary ports, and allowing only trusted applications." },
{ "Can a firewall block all cyber attacks?", "A firewall is a strong defense, but it cannot block all attacks. Use it alongside other security measures for best protection." },

//antivirus follow up questions
{ "How often should I run antivirus scans?", "Run antivirus scans at least once a week and enable real-time protection for continuous monitoring." },
{ "What features should I look for in antivirus software?", "Look for real-time protection, automatic updates, malware removal, and web protection features in antivirus software." },
{ "Can antivirus software detect all threats?", "No antivirus can detect all threats, but keeping it updated and using safe browsing habits greatly reduces your risk." },

//password follow up questions
{ "How do I create a strong password?", "Create a strong password by using at least 12 characters, mixing uppercase, lowercase, numbers, and symbols, and avoiding common words." },
{ "What is a password manager?", "A password manager is a tool that securely stores and manages your passwords, helping you use unique passwords for every account." },
{ "How often should I change my passwords?", "Change your passwords regularly, especially if you suspect a breach or if the service recommends it." },

//social engineering follow up questions
{ "What are common social engineering techniques?","Common techniques include phishing, pretexting, baiting, and impersonation to trick people into revealing information." },
{ "How can I avoid falling victim to social engineering?","Be cautious with unsolicited requests, verify identities, and never share sensitive information without confirmation." },
{ "Can social engineering happen over the phone?","Yes, attackers often use phone calls to impersonate trusted individuals and extract confidential information." }


  };


      Dictionary<string, List<string>> keywordGroups = new Dictionary<string, List<string>>
{
{ "cybersecurity", new List<string> { "cyber security", "online safety", "information security", "cyber attack" } },
{ "phishing", new List<string> { "phishing emails", "phishing", "fake emails", "email scam", "spear phishing", "phishing attack" } },
{ "malware", new List<string> { "malicious software", "virus", "trojan", "worm", "spyware", "adware", "malware infection" } },
{ "ransomware", new List<string> { "ransomware attack", "encrypted files", "pay ransom", "ransom demand" } },
{ "firewall", new List<string> { "network firewall", "firewall protection", "block traffic", "security firewall" } },
{ "antivirus", new List<string> { "antivirus software", "virus protection", "antimalware", "scan for viruses" } },
{ "password", new List<string> { "strong password", "password security", "password manager", "password protection" } },
{ "social engineering", new List<string> { "social engineering attack", "manipulation", "pretexting", "baiting", "impersonation" } },

//cybersecurity follow up question synonyms
{ "What are the most common cyber threats?", new List<string> { "common cyber threats", "most common cyber threats","cyber threats" } },
{ "How can I protect my personal information online?", new List<string> { "protect personal information", "protect my personal information" } },
{ "What is the importance of cybersecurity in daily life?", new List<string> { "importance of cybersecurity", "importance of cyber security" } },

//phishing follow up question synonyms
{ "How can I recognize a phishing email?", new List<string> { "recognize phishing email", "phishing email signs" } },
{ "What should I do if I clicked a phishing link?", new List<string> { "clicked phishing link", "clicked on a phishing link" } },
{ "Are there tools to help prevent phishing attacks?", new List<string> { "tools to prevent phishing", "prevent phishing attacks" } },

//malware follow up question synonyms
{ "What are the signs of a malware infection?", new List<string> { "signs of malware infection", "malware infection signs" } },
{ "How do I remove malware from my computer?", new List<string> { "remove malware from computer", "remove malware" } },
{ "What types of malware exist?", new List<string> { "types of malware", "different types of malware" } },

//ransomware follow up question synonyms
{ "What should I do if my files are encrypted by ransomware?", new List<string> { "files encrypted by ransomware", "ransomware encrypted files" } },
{ "How can I prevent ransomware attacks?", new List<string> { "prevent ransomware attacks", "ransomware prevention" } },
{ "Should I pay the ransom if infected?", new List<string> { "pay ransom if infected", "pay ransom" } },

//firewall follow up question synonyms
{ "What is the difference between hardware and software firewalls?", new List<string> { "difference between hardware and software firewalls", "hardware vs software firewall" } },
{ "How do I configure a firewall?", new List<string> { "configure a firewall", "firewall configuration" } },
{ "Can a firewall block all cyber attacks?", new List<string> { "firewall block all cyber attacks", "firewall protection" } },

//antivirus follow up question synonyms
{ "How often should I run antivirus scans?", new List<string> { "run antivirus scans", "antivirus scan frequency" } },
{ "What features should I look for in antivirus software?", new List<string> { "features of antivirus software", "antivirus software features" } },
{ "Can antivirus software detect all threats?", new List<string> { "antivirus detect all threats", "antivirus software detection" } },

//password follow up question synonyms
{ "How do I create a strong password?", new List<string> { "create strong password", "strong password tips" } },
{ "What is a password manager?", new List<string> { "password manager", "what is a password manager" } },
{ "How often should I change my passwords?", new List<string> { "change passwords frequency", "how often to change passwords" } },

//social engineering follow up question synonyms

{ "What are common social engineering techniques?", new List<string> { "common social engineering techniques", "social engineering techniques" } },
{ "How can I avoid falling victim to social engineering?", new List<string> { "avoid social engineering", "prevent social engineering" } },
{ "Can social engineering happen over the phone?", new List<string> { "social engineering over the phone", "phone social engineering" } },


};

            //Check if the input contains any of the keywords
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
                RespondWithSpeech("Sorry, I don't understand. Type 'help' to see what you can ask. ");
            }
        }





        //Display a random security tip
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
