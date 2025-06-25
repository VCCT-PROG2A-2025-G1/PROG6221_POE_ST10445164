
This is **Part 2** of the PROG6221 Programming 2A Portfolio of Evidence. CyberBot is a console-based chatbot designed to educate South African users about cybersecurity in an engaging, interactive way. In this part, the chatbot evolves into a more natural, intelligent assistant through enhanced keyword recognition, memory, sentiment detection, and conversational logic.
Video Link: https://youtu.be/HsLuYI9lZi4

---

## 📌 Objective

To extend the functionality of the Part 1 chatbot by:
- Recognising cybersecurity keywords
- Responding with varied, dynamic content
- Detecting user sentiment
- Remembering user preferences
- Creating a smooth, flowing chat experience

---

## 🛠️ Features Added in Part 2

### 🧠 Keyword Recognition
- The chatbot detects and responds to topics like:
  - **Password safety**
  - **Phishing and scams**
  - **Privacy risks**
- Uses dictionaries and lists to match keywords to appropriate cybersecurity tips.

### 🔁 Randomized Responses
- Responses to key cybersecurity questions are randomly selected from a list for variety.
- Example: Phishing tips vary with each conversation to keep interactions fresh.

### 💬 Memory and Recall
- Remembers:
  - The user's name
  - Topics the user is interested in (e.g., "I'm interested in privacy.")
- Uses memory to personalize future responses:
  - "As someone interested in privacy, make sure to review your account settings."

### ❤️ Sentiment Detection
- Detects and reacts to emotional tones in user input such as:
  - **"Worried"**, **"anxious"** → Reassures the user.
  - **"Frustrated"**, **"upset"** → Offers encouragement.
  - **"Curious"**, **"interested"** → Promotes learning and exploration.
- Responds by extracting and highlighting only the emotional keyword, not the full user input.

### 🔄 Smooth Conversation Flow
- Maintains context across multiple questions.
- Handles follow-ups like:
  - "Tell me more"
  - "Can you explain"
  - Without losing the previous topic.

### 🧰 Error Handling & Validation
- Gracefully handles unrecognized inputs and prompts the user to rephrase.
- Prevents empty or invalid commands from causing crashes.

### 🧼 Code Structure & Optimisation
- Clean, modular design using:
  - `Dictionary<string, List<string>>` for keyword-to-response mapping
  - `List<string>` for remembered topics
  - `switch` cases with `when` conditions and methods for reusable sentiment logic
