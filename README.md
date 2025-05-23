The chatbot works by playing an audible welcome message and then displays an ASCII art logo for the chatbot. 
The user is then prompted for their name so that the chatbot can use it when it reponds to give a more friendly appeal to the conversation.
The chatbot then greets the user using the name that the user inputed. This is saved to a variable called userName.
Then, the chatbot displays a 'security tip of the day' which is chosen randomly from a list of tips stored in a method called DisplayTipOfTheDay() .
Beneath the security tip, the chatbot also displays a list of cybersecurity topics that the user will be able to ask about.
The user can now enter a question or statement for the chatbot can answer to.
If the user asks about a topic that was displayed in the previous list, the chatbot will display a reponse that is stored in a method called HandleUserQuery() .
If the user does not ask about a topic that was displayed in the previous list, the chatbot will display a message telling the user that it did not understand and that they can enter the word 'help' to see what the user can ask about.
However, before the error message displays, the chatbot will display a message saying that it will remember what the user asked about for another conversation, as this will help the converstaion feel more natural
Everything that the chatbot and the user says in a conversation is saved to a text document so that the chatbot history will be saved for referencing in for another conversation.
If the user enters a topic that is from the list of topics that chatbot can answer and they have asked about this topic in before, the chatbot will give its predertermined response from HandleUserQuery() after the chatbot mentions that they have had previous interest in this topic before
If the user enters the word 'exit' or 'quit', the chatbot will display a goodbye message, it will display a message telling the user that the conversation was saved to the chatbot's history and then the program will end.
