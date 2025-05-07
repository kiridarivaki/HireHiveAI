using Azure;
using Azure.AI.Inference;

namespace HireHive.Infrastructure.Services
{
    public class AiAssessmentService
    {
        private readonly ChatCompletionsClient _client;
        private readonly PiiRedactionService _piiRedactionService;
        public AiAssessmentService(ChatCompletionsClient client, PiiRedactionService piiRedactionService)
        {
            _client = client;
            _piiRedactionService = piiRedactionService;
        }

        public async Task Chat(string blobName)
        {
            string prompt = "You are an AI model that analyzes the following documents and provides a summary of key points.";

            string document = "{\r\n  \"fullName\": \"Ashley Gill\",\r\n  \"email\": \"ashleygill2023@gotmail.com\",\r\n  \"education\": [\r\n    {\r\n      \"degree\": \"BA International Business Studies with Spanish (expected 2:1)\",\r\n      \"institution\": \"Buckinghamshire Edge University\",\r\n      \"year\": \"2008 – present\",\r\n      \"details\": [\r\n        \"Study semester at The University of Valloid (Spain)\",\r\n        \"Six-month work placement in Madrid\",\r\n        \"Relevant modules: Business Planning; Sales Promotion and Marketing; and Business Operations Management\"\r\n      ]\r\n    },\r\n    {\r\n      \"degree\": \"A-Levels: Business Studies (B), French (C)\",\r\n      \"institution\": \"Freebridge School\",\r\n      \"year\": \"2000 – 2007\"\r\n    },\r\n    {\r\n      \"degree\": \"8 GCSEs including Maths, English, Spanish and French\",\r\n      \"institution\": \"Freebridge School\",\r\n      \"year\": \"2000 – 2007\"\r\n    }\r\n  ],\r\n  \"workExperience\": [\r\n    {\r\n      \"jobTitle\": \"Librarian/tour guide\",\r\n      \"company\": \"Buckinghamshire Edge University\",\r\n      \"startDate\": \"2008 – 2011\",\r\n      \"endDate\": \"\",\r\n      \"responsibilities\": [\r\n        \"General administrative and customer service roles.\"\r\n      ]\r\n    },\r\n    {\r\n      \"jobTitle\": \"Audit Assistant\",\r\n      \"company\": \"Audigest S.A. (Madrid)\",\r\n      \"startDate\": \"2011 (Feb–Aug)\",\r\n      \"endDate\": \"\",\r\n      \"responsibilities\": [\r\n        \"Six months’ work experience in an international bank.\",\r\n        \"Liaising with colleagues and clients in English and Spanish.\"\r\n      ]\r\n    },\r\n    {\r\n      \"jobTitle\": \"Supervisor\",\r\n      \"company\": \"Finsbury’s supermarket (Hazelbridge)\",\r\n      \"startDate\": \"2010 (June–Dec)\",\r\n      \"endDate\": \"\",\r\n      \"responsibilities\": [\r\n        \"Managing a small team.\",\r\n        \"Customer service in a busy competitive environment.\"\r\n      ]\r\n    },\r\n    {\r\n      \"jobTitle\": \"Financial Assistant/Supervisor\",\r\n      \"company\": \"Top Choice Holidays and Flights Ltd (Low Wycombe)\",\r\n      \"startDate\": \"2010 (Jan–Aug)\",\r\n      \"endDate\": \"\",\r\n      \"responsibilities\": [\r\n        \"Working in a range of teams to manage complex financial processes.\"\r\n      ]\r\n    },\r\n    {\r\n      \"jobTitle\": \"General Assistant\",\r\n      \"company\": \"Dogs Protection League\",\r\n      \"startDate\": \"2007 (Jul–Aug)\",\r\n      \"endDate\": \"\",\r\n      \"responsibilities\": [\r\n        \"Dealing with enquiries and selling packages to a range of clients.\"\r\n      ]\r\n    },\r\n    {\r\n      \"jobTitle\": \"Supervisor\",\r\n      \"company\": \"McHenry’s Restaurant (Low Wycombe)\",\r\n      \"startDate\": \"2006 (Jan–Dec)\",\r\n      \"endDate\": \"\",\r\n      \"responsibilities\": [\r\n        \"Managing a small team.\",\r\n        \"Customer service.\"\r\n      ]\r\n    }\r\n  ],\r\n  \"certifications\": [\r\n    {\r\n      \"title\": \"Spanish fluency\",\r\n      \"organization\": \"Self-Study/Experience in Spain\",\r\n      \"year\": \"2010-2011\"\r\n    },\r\n    {\r\n      \"title\": \"French - semi-fluent\",\r\n      \"organization\": \"Self-Study\",\r\n      \"year\": \"Ongoing\"\r\n    },\r\n    {\r\n      \"title\": \"Bookkeeping (evening course)\",\r\n      \"organization\": \"Self-funded\",\r\n      \"year\": \"During first accountancy role\"\r\n    }\r\n  ],\r\n  \"skills\": [\r\n    \"Effective communication\",\r\n    \"Customer service\",\r\n    \"Teamwork\",\r\n    \"Administration\",\r\n    \"Experience of travellers’ needs\",\r\n    \"Initiative\",\r\n    \"Sales knowledge\",\r\n    \"Language ability: Spanish (fluent), French (semi-fluent)\"\r\n  ]\r\n}\r\n"

            string description = "Job Title: Software Engineer (Backend)\r\nLocation: Hybrid – New York, NY\r\nType: Full-time\r\n\r\nAbout the Role:\r\nWe’re seeking a backend-focused Software Engineer to join our team. You'll build scalable APIs and microservices using C# and .NET, helping power our data platform.\r\n\r\nKey Responsibilities:\r\nDevelop and maintain backend services in .NET 6+\r\n\r\nCollaborate with cross-functional teams\r\n\r\nWrite clean, tested, and maintainable code\r\n\r\nTroubleshoot production issues\r\n\r\nRequirements:\r\n3+ years of experience with C#/.NET\r\n\r\nProficient in SQL/NoSQL databases\r\n\r\nFamiliarity with Docker, CI/CD, and cloud platforms (AWS/Azure)\r\n\r\nBonus Points:\r\nKnowledge of Kafka, GraphQL, or DDD\r\n\r\nPerks:\r\nCompetitive salary + benefits\r\n\r\nFlexible PTO & remote work\r\n\r\nHealth, dental, 401(k), and more";
            var requestOptions = new ChatCompletionsOptions()
            {
                Messages =
            {
                new ChatRequestSystemMessage("you are a cv hr specialist. You match a candidate to a job description. You answer with the percentage level to which the candidate matches to the job and a small explanation."),
                new ChatRequestUserMessage(document),
                new ChatRequestUserMessage(description)
            },
                Temperature = 0.7f,
                NucleusSamplingFactor = 1.0f,
                Model = "openai/gpt-4.1"
            };

            Response<ChatCompletions> response = _client.Complete(requestOptions);
        }
    }
}
