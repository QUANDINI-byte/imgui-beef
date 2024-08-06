using System;
using DirectX.Windows;

namespace ImGui
{
    public static class ImGuiImplWin32
    {
        /* this is the callback function for input from a win32 window.
        here is an example of how to use it:
        public static LRESULT MessageHandler(HWND hWnd, uint32 uMsg, WPARAM wParam, LPARAM lParam)
        {
    	    LRESULT ImGuiR = ImGui.ImGuiImplWin32.WndProcHandler(hWnd, uMsg, wParam, lParam);
    	    if (ImGuiR != 0)
    	    {
    		    return ImGuiR;
    	    }
    	
    	    ...
        } */
        [CallingConvention(.Stdcall), LinkName("ImGui_ImplWin32_WndProcHandler")]
        public static extern LRESULT WndProcHandlerImp(HWND hWnd, uint32 uMsg, WPARAM wParam, LPARAM lParam);
        public static LRESULT WndProcHandler(HWND hWnd, uint32 uMsg, WPARAM wParam, LPARAM lParam) => WndProcHandlerImp(hWnd, uMsg, wParam, lParam);
    
        [LinkName("ImGui_ImplWin32_EnableAlphaCompositing")]
        private static extern void EnableAlphaCompositingImpl(void* hwnd);
        public static void EnableAlphaCompositing(void* hwnd) => EnableAlphaCompositingImpl(hwnd);
        
        [LinkName("ImGui_ImplWin32_EnableDpiAwareness")]
        private static extern void EnableDpiAwarenessImpl();
        public static void EnableDpiAwareness() => EnableDpiAwarenessImpl();
        
        [LinkName("ImGui_ImplWin32_GetDpiScaleForHwnd")]
        private static extern float GetDpiScaleForHwndImpl(void* hwnd);
        public static float GetDpiScaleForHwnd(void* hwnd) => GetDpiScaleForHwndImpl(hwnd);
        
        [LinkName("ImGui_ImplWin32_GetDpiScaleForMonitor")]
        private static extern float GetDpiScaleForMonitorImpl(void* monitor);
        public static float GetDpiScaleForMonitor(void* monitor) => GetDpiScaleForMonitorImpl(monitor);
        
        [LinkName("ImGui_ImplWin32_Init")]
        private static extern bool InitImpl(void* hwnd);
        public static bool Init(void* hwnd) => InitImpl(hwnd);
        
        [LinkName("ImGui_ImplWin32_InitForOpenGL")]
        private static extern bool InitForOpenGLImpl(void* hwnd);
        public static bool InitForOpenGL(void* hwnd) => InitForOpenGLImpl(hwnd);
        
        [LinkName("ImGui_ImplWin32_NewFrame")]
        private static extern void NewFrameImpl();
        public static void NewFrame() => NewFrameImpl();
        
        [LinkName("ImGui_ImplWin32_Shutdown")]
        private static extern void ShutdownImpl();
        public static void Shutdown() => ShutdownImpl();
    }
}