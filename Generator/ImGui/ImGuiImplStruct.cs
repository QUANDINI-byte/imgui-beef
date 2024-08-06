using System.Collections.Generic;
using System.Linq;

namespace ImGuiBeefGenerator.ImGui
{
    public class ImGuiImplStruct : IBinding
    {
        public string Name { get; }
        public List<ImGuiMethodDefinition> Methods { get; }

        public ImGuiImplStruct(string name)
        {
            Name = name;
            Methods = new List<ImGuiMethodDefinition>();
        }

        public static List<ImGuiImplStruct> From(Dictionary<string, object> implDefinitions)
        {
            var structs = new List<ImGuiImplStruct>();

            foreach (var definition in implDefinitions)
            {
                var structName = definition.Key.Replace("ImGui_Impl", "ImGuiImpl");
                var definitionName = structName.Substring(structName.IndexOf('_') + 1);
                structName = structName.Replace($"_{definitionName}", "");

                if (!structs.Any(s => s.Name == structName))
                    structs.Add(new ImGuiImplStruct(structName));

                var implStruct = structs.Single(s => s.Name == structName);
                var variation = (Dictionary<string, object>) (definition.Value as List<object>).First();
                variation["funcname"] = definitionName;
                implStruct.Methods.Add(ImGuiMethodDefinition.FromVariation(variation));
            }

            return structs;
        }

        public string Serialize()
        {
            var serialized = 
$@"
public static class {Name}
{{";

            if (Name == "ImGuiImplGlfw")
            {
                serialized +=
@"
    private typealias GLFWwindow = GLFW.GlfwWindow;
    private typealias GLFWmonitor = GLFW.GlfwMonitor;
";
            }
            else if (Name.StartsWith("ImGuiImplOpenGL"))
            {
                serialized +=
@"
    private typealias char = char8;
    private typealias DrawData = ImGui.DrawData;

	[LinkName(""gladLoadGL"")]
    private static extern int GladLoadGL();
";
            }
            else if (Name == "ImGuiImplSDL2")
            {
                serialized +=
@"
    private typealias SDL_Window = SDL2.SDL.Window;
    private typealias SDL_Event = SDL2.SDL.Event;
    private typealias SDL_Renderer = SDL2.SDL.Renderer;
";
            }
            else if (Name == "ImGuiImplWin32")
            {
                serialized +=
$@"
    /* this is the callback function for input from a win32 window.
    here is an example of how to use it:
    public static LRESULT MessageHandler(HWND hWnd, uint32 uMsg, WPARAM wParam, LPARAM lParam)
    {{
	    LRESULT ImGuiR = ImGui.ImGuiImplWin32.WndProcHandler(hWnd, uMsg, wParam, lParam);
	    if (ImGuiR != 0)
	    {{
		    return ImGuiR;
	    }}
	
	    ...
    }} */
    [CallingConvention(.Stdcall), LinkName(""ImGui_ImplWin32_WndProcHandler"")]
    public static extern LRESULT WndProcHandlerImp(HWND hWnd, uint32 uMsg, WPARAM wParam, LPARAM lParam);
    public static LRESULT WndProcHandler(HWND hWnd, uint32 uMsg, WPARAM wParam, LPARAM lParam) => WndProcHandlerImp(hWnd, uMsg, wParam, lParam);
";
            }

            foreach (var method in Methods)
                serialized += method.Serialize().Replace("\n", "\n    ");

            serialized = serialized.Remove(serialized.Length - 4);
            serialized += "}\n";
            return serialized;
        }
    }
}
