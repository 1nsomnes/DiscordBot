using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;

namespace DiscordBot.Modules.Utilities
{
    public class HelpingCommands : ModuleBase<SocketCommandContext>
    {
        [Command("codeblock")]
        [Alias("cb")]
        public async Task CodeBlock(params string[] args)
        {
            await ReplyAsync("**__Use codeblocks to send code in a message!__**\n\n" +
                "To make a codeblock, surround your code with \\`\\`\\`\n" +
                "To use C# syntax highlighting add `cs` after the three back ticks like so:\n\n" +
                "\\`\\`\\`cs\n" +
                "// your code here\n" +
                "\\`\\`\\`\n\n" +
                "To send lengthy code, paste it into <https://paste.myst.rs/> and send the link of the paste into chat.\n");
        }

        [Command("imageofcode")]
        [Alias("ioc")]
        public async Task ImageOfCode(params string[] args)
        {
            await ReplyAsync("**__An image of your code is not helpful__**\n\n" +
                "When asking a question about a problem with code, people who are volunteering to help need the text of the code.Unless you are asking about your IDE - and not the code itself - images of the code are not an acceptable substitute.\n" +
                "Source: <https://idownvotedbecau.se/imageofcode> \n\n" +
                $"Please send your code as a codeblock.If you don't know how to send a codeblock, type `{ConfigLoader.Prefix}cb`.");
        }

        [Command("itsnotworking")]
        public async Task ItsNotWorking(params string[] args)
        {
            await ReplyAsync("**__\"It's not working\" is not helpful__**\n\n" +
                "In order for a question to be answered, it must specify what exactly is wrong.Stating simply that \"it doesn't work\" is not sufficient.\n" +
                "Source: <https://idownvotedbecau.se/itsnotworking> \n\n" +
                "Please elaborate on your question by including all relevant details.What do you think is the problem ? Have you tried to fix it? If you have, why didn't that work?");
        }

        [Command("nocode")]
        public async Task NoCode(params string[] args)
        {
            await ReplyAsync("**__It's hard to answer a programming question without code__**\n\n" +
                "Resolving a bug is almost impossible when the question doesn't include any of the buggy code. In order to help fix the problem, answerers are going to have to see what the code is.\n" +
                "Source: <https://idownvotedbecau.se/nocode> \n\n" +
                $"Please isolate the problematic code and send it as a codeblock. If you don't know how to send a codeblock, type `{ConfigLoader.Prefix}cb`");
        }

        [Command("noint")]
        public async Task NoInt(params string[] args)
        {
            await ReplyAsync("**__How to deal with no VS intellisense problem:__**\n\n" +
                "If you have opened script and see `Miscellaneous Files` in place where should be name of your project(see screenshot below) it means that the file was not correctly loaded into project or you don't have any project loaded. Follow these steps to fix this:\n" +
                "- Unity tools and Unity IDE preference - make sure to install the Unity Tools in your VS Installer and check the Unity preferences `Edit - Preferences - External Tools` and verify that the right External Script Editor is selected(you want Visual Studio there).\n" +
                "-Reload project - `View - Solution Explorer` right click on project and click `Unload project` then right click on it again and load it again.If you don't see any project in Solution Explorer then move to the next step.\n" +
                "- Regenerate solution files - Remove the `.sln` and `.csproj` files from root of your unity project(in File Explorer), they will be regenerated next time you open script from Unity.\n\n" +
                "How to deal with no VSCode intellisense problem:\n" +
                "Follow the guidelines on this page:\n" +
                "<https://code.visualstudio.com/docs/other/unity>\n" +
                "https://cdn.discordapp.com/attachments/545801512173961239/553525896045133826/unknown.png");
        }

        [Command("xyproblem")]
        public async Task XYProblem(params string[] args)
        {
            await ReplyAsync("**__The XY Problem__**\n\n" +
                "The XY problem is asking about your attempted solution rather than your actual problem.This leads to enormous amounts of wasted time and energy, both on the part of people asking for help, and on the part of those providing help.\n\n" +
                "- You want to do X\n" +
                "- You don't know how to do X, but think you can solve it if you can just about manage to do Y\n" +
                "- You don't know how to do Y either, so you ask for help with Y\n" +
                "- Others try to help you with Y, but are confused because Y seems like a strange problem to want to solve\n" +
                "-After much interaction and wasted time, it finally becomes clear that the you really want help with X, and that Y wasn't even a suitable solution for X\n" +
                "Source: http://xyproblem.info/ \n\n" +
                "Please include information about a broader picture along with any attempted solution, including solutions you have already ruled out and why.");
        }

        [Command("unclearquestion")]
        public async Task UnclearQuestion(params string[] args)
        {
            await ReplyAsync("**__Before others can help, a clear question must be formulated__**\n\n" +
                "When you ask a difficult question it is your responsibility to ensure that anyone reading it will have all of the information they need to understand and diagnose the problem.Sometimes questions aren’t as clear to others as they could be or they may be missing critical information needed to provide a correct answer.\n" +
                "Source: <https://idownvotedbecau.se/unclearquestion> \n\n" +
                "Please elaborate on your question by including all the relevant information such as:\n" +
                "-The programming language within which you're working (if it is anything other than C#)\n" +
                "- Exactly what it is you're trying to accomplish\n" +
                "- Things you have considered / attempted already\n" +
                "- Anything else that could aid answerers in resolving the issue");
        }

        [Command("noresearch")]
        public async Task NoResearch(params string[] args)
        {
            await ReplyAsync("**__Research is an important first step in solving problems__**\n\n" +
                "Solving problems can be hard work.When we have exhausted our own knowledge, it's often tempting to simply ask someone else to solve the problem for us. It is very common to find the question we have is one many people have already experienced, and many of these people have already asked about it and have received correct answers in response.\n" +
                "Source: <https://idownvotedbecau.se/noresearch> \n\n" +
                "If you've done your research, please state what you've already attempted in your question to avoid being offered a solution that you've already attempted. This saves time on everybody's part!");
        }

        [Command("imageofanexception")]
        [Alias("ioe")]
        public async Task ImageOfAnException(params string[] args)
        {
            await ReplyAsync("**__Pictures of exceptions are not helpful__**\n\n" +
                "Pasting a picture of an error or an exception is not helpful.It is not required to prove that the error happened – you are trusted when you state this fact within the question.It is the details omitted by these images that is the problem, as these images only contain part of the information.\n" +
                "Source: <https://idownvotedbecau.se/imageofanexception>\n\n" +
                "Please copy and paste the error as text so that we can further understand all of the relevant information, and point out the exact line that is throwing the error, as this saves the time of those who are helping you!");
        }

        [Command("screenshot")]
        public async Task ScreenShot(params string[] args)
        {
            await ReplyAsync("**__Capturing a screenshot on Windows 10__**\n" +
                "Use **Win + Shift + S**, select a region of your screen, and then paste it into Discord using **Ctrl+V**.\n" +
                "*(Works only on Windows 10 - has to have an update later than April 2017).*\n" +
                "**__Capturing a screenshot on macOS__**\n" +
                "Use **Shift + Cmd + 4**, select a region of your screen, and then paste it into Discord using **Cmd+V**.\n\n" +
                "**__Capturing a screenshot on Linux__**\n" +
                "The screenshot shortcut may vary between distros.\n" +
                "If you are using Ubuntu: use **Shift+PrintScreen**, select a region of your screen, and then paste it into Discord using **Ctrl+V**.");
        }

        [Command("getareference")]
        [Alias("getref")]
        public async Task GetAReference(params string[] args)
        {
            await ReplyAsync("**__How to get a reference__**\n\n" +
                "\"How do I access a variable from another script?\" is usually one of the first questions we ask when learning Unity.Whether you are C# beginner or a C# professional, understanding the way Unity does things differently can be confusing.\n\n" +
                ":deciduous_tree: __If the objects are already present in the scene hierarchy...__" +
                "... then you can use the `SerializeField` attribute on a field, and assign it by dragging the object which has the script you want to access onto the field slot.This also works if the two scripts are on the same object.\n" +
                "```cs\n" +
                "[SerializeField] private SomeScript someScript;\n" +
                "```\n\n" +
                ":blue_square:  __If the objects are instantiated prefabs...__\n" +
                "...then you can use a form of injection after you call `Instantiate`:\n" +
                "`SomeScript.cs`\n" +
                "```cs\n" +
                "public GameManager TheManager { get; set; }\n" +
                "```\n\n" +
                "`GameManager.cs`\n" +
                "```cs\n" +
                "var clone = Instantiate(prefab);\n" +
                "clone.GetComponent<SomeScript>().TheManager = this;\n" +
                "```");
        }

        [Command("learnc#")]
        [Alias("learn")]
        public async Task LearnCSharp(params string[] args)
        {
            await ReplyAsync("**__Learn C# basics before starting with Unity!__**\n\n" +
                "Learning the syntax of C# definitely helps when using Unity. Here are some links to get you started!\n" +
                "- <https://docs.microsoft.com/en-us/dotnet/csharp/> (Microsofts 'Getting Started' Guide on C#)\n" +
                "- <https://channel9.msdn.com/Series/CSharp-Fundamentals-for-Absolute-Beginners> (Teaches you the C# fundamentals)\n" +
                "- <https://github.com/ossu/computer-science> (Not strictly C#, a general open-source education in Computer Science)\n" +
                "- <https://www.classcentral.com/report/stanford-on-campus-courses/?utm_source=cc_newsletter&utm_medium=email&utm_campaign=november2020> (Publicly available Computer Science courses from Stanford)\n\n" +
                "Most programming problems come from not knowing how to use the language - if you haven’t programmed much or you’re not confident about the OOP concepts in your mind, it's useful to understand these before diving into the engine.");
        }

        [Command("nofind")]
        public async Task NoFind(params string[] args)
        {
            await ReplyAsync("**__Don't use `Find` methods!__**\n\n" +
                "`Find` methods in Unity are an often - tempting solution to referencing a scene object.However, there is always a better solution.\n" +
                "Whenever you call a `Find` method, Unity must traverse your entire scene hierarchy and check every single object until it finds a match; and the methods which return arrays will always traverse the entire hierarchy regardless.\n" +
                "This is inefficient!It means that the more objects you add to your scene, **the slower these methods become**; and it gets even worse if you are calling them multiple times.\n\n" +
                "A non-exhaustive list of `Find` methods to __avoid__:\n" +
                "<https://docs.unity3d.com/ScriptReference/GameObject.FindWithTag.html>\n" +
                "<https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html>\n" +
                "<https://docs.unity3d.com/ScriptReference/Object.FindObjectsOfType.html>\n" +
                "<https://docs.unity3d.com/ScriptReference/Object.FindObjectOfType.html>\n\n" +
                $"To read about a better solution, type `{ConfigLoader.Prefix}getareference` or `{ConfigLoader.Prefix}getref`");
        }

        [Command("nullreference")]
        [Alias("nullref")]
        public async Task NullReference(params string[] args)
        {
            await ReplyAsync("**NullReferenceException** means that you either never assigned an object to variable, or set it to null.\n\n" +
                "Do `Debug.Log(yourVariable == null)` before the line with the error to test if that's the case.\n\n" +
                "You can look into:\n" +
                "• <https://docs.microsoft.com/en-gb/dotnet/api/system.nullreferenceexception>\n" +
                "• <https://docs.unity3d.com/Manual/NullReferenceException.html>");
        }

        [Command("projectideas")]
        [Alias("idea", "ideas", "ideagenerator", "projectidea")]
        public async Task ProjectIdeas(params string[] args)
        {
            await ReplyAsync("These are some links with ideas for small projects!\n" +
                "<https://github.com/karan/Projects>\n" +
                "<https://github.com/joereynolds/what-to-code>\n" +
                "<http://andrewcombs13.com/projectIdeas/>\n" +
                "<https://www.reddit.com/r/dailyprogrammer/>\n" +
                "<https://qph.fs.quoracdn.net/main-qimg-2fa8b318be75adf3d32f85eb42d2422e-c>\n" +
                "<https://github.com/scraggo/bookmarks-programming/blob/master/project-ideas.md>\n" +
                "<https://rosettacode.org/wiki/Category:Programming_Tasks>");
        }
    }
}
