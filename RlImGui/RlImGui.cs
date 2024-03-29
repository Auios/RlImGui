namespace Auios.ImGui;

using System.Numerics;
using ImGuiNET;
using Raylib_CsLo;

public static class RlImGui
{
    internal static IntPtr ImGuiContext = IntPtr.Zero;

    private static ImGuiMouseCursor currentMouseCursor = ImGuiMouseCursor.COUNT;
    private static Dictionary<ImGuiMouseCursor, MouseCursor> mouseCursorMap;

    private static Texture fontTexture;

    public static void Setup(bool darkTheme = true)
    {
        RlImGui.mouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();

        RlImGui.fontTexture.id = 0;

        RlImGui.BeginInitImGui();

        if (darkTheme)
            ImGui.StyleColorsDark();
        else
            ImGui.StyleColorsLight();

        RlImGui.EndInitImGui();
    }

    public static void BeginInitImGui()
    {
        RlImGui.ImGuiContext = ImGui.CreateContext();
    }

    private static void SetupMouseCursors()
    {
        RlImGui.mouseCursorMap.Clear();
        RlImGui.mouseCursorMap[ImGuiMouseCursor.Arrow] = MouseCursor.MOUSE_CURSOR_ARROW;
        RlImGui.mouseCursorMap[ImGuiMouseCursor.TextInput] = MouseCursor.MOUSE_CURSOR_IBEAM;
        RlImGui.mouseCursorMap[ImGuiMouseCursor.Hand] = MouseCursor.MOUSE_CURSOR_POINTING_HAND;
        RlImGui.mouseCursorMap[ImGuiMouseCursor.ResizeAll] = MouseCursor.MOUSE_CURSOR_RESIZE_ALL;
        RlImGui.mouseCursorMap[ImGuiMouseCursor.ResizeEW] = MouseCursor.MOUSE_CURSOR_RESIZE_EW;
        RlImGui.mouseCursorMap[ImGuiMouseCursor.ResizeNESW] = MouseCursor.MOUSE_CURSOR_RESIZE_NESW;
        RlImGui.mouseCursorMap[ImGuiMouseCursor.ResizeNS] = MouseCursor.MOUSE_CURSOR_RESIZE_NS;
        RlImGui.mouseCursorMap[ImGuiMouseCursor.ResizeNWSE] = MouseCursor.MOUSE_CURSOR_RESIZE_NWSE;
        RlImGui.mouseCursorMap[ImGuiMouseCursor.NotAllowed] = MouseCursor.MOUSE_CURSOR_NOT_ALLOWED;
    }

    public static unsafe void ReloadFonts()
    {
        ImGui.SetCurrentContext(RlImGui.ImGuiContext);
        ImGuiIOPtr io = ImGui.GetIO();

        IntPtr pixels;
        int width, height, bytesPerPixel;
        io.Fonts.GetTexDataAsRGBA32(out pixels, out width, out height, out bytesPerPixel);

        Image image = Raylib.GenImageColor(width, height, Raylib.BLANK);

        RlImGui.fontTexture = Raylib.LoadTextureFromImage(image);
        Raylib.UpdateTexture(RlImGui.fontTexture, (void*)pixels);

        io.Fonts.SetTexID(new IntPtr(RlImGui.fontTexture.id));
    }

    public static void EndInitImGui()
    {
        RlImGui.SetupMouseCursors();

        ImGui.SetCurrentContext(RlImGui.ImGuiContext);
        // ImFontAtlasPtr fonts = ImGui.GetIO().Fonts;
        ImGui.GetIO().Fonts.AddFontDefault();

        ImGuiIOPtr io = ImGui.GetIO();
        io.KeyMap[(int)ImGuiKey.Tab] = (int)KeyboardKey.KEY_TAB;
        io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)KeyboardKey.KEY_LEFT;
        io.KeyMap[(int)ImGuiKey.RightArrow] = (int)KeyboardKey.KEY_RIGHT;
        io.KeyMap[(int)ImGuiKey.UpArrow] = (int)KeyboardKey.KEY_UP;
        io.KeyMap[(int)ImGuiKey.DownArrow] = (int)KeyboardKey.KEY_DOWN;
        io.KeyMap[(int)ImGuiKey.PageUp] = (int)KeyboardKey.KEY_PAGE_UP;
        io.KeyMap[(int)ImGuiKey.PageDown] = (int)KeyboardKey.KEY_PAGE_DOWN;
        io.KeyMap[(int)ImGuiKey.Home] = (int)KeyboardKey.KEY_HOME;
        io.KeyMap[(int)ImGuiKey.End] = (int)KeyboardKey.KEY_END;
        io.KeyMap[(int)ImGuiKey.Delete] = (int)KeyboardKey.KEY_DELETE;
        io.KeyMap[(int)ImGuiKey.Backspace] = (int)KeyboardKey.KEY_BACKSPACE;
        io.KeyMap[(int)ImGuiKey.Enter] = (int)KeyboardKey.KEY_ENTER;
        io.KeyMap[(int)ImGuiKey.Escape] = (int)KeyboardKey.KEY_ESCAPE;
        io.KeyMap[(int)ImGuiKey.Space] = (int)KeyboardKey.KEY_SPACE;
        io.KeyMap[(int)ImGuiKey.A] = (int)KeyboardKey.KEY_A;
        io.KeyMap[(int)ImGuiKey.C] = (int)KeyboardKey.KEY_C;
        io.KeyMap[(int)ImGuiKey.V] = (int)KeyboardKey.KEY_V;
        io.KeyMap[(int)ImGuiKey.X] = (int)KeyboardKey.KEY_X;
        io.KeyMap[(int)ImGuiKey.Y] = (int)KeyboardKey.KEY_Y;
        io.KeyMap[(int)ImGuiKey.Z] = (int)KeyboardKey.KEY_Z;

        RlImGui.ReloadFonts();
    }

    private static void NewFrame()
    {
        ImGuiIOPtr io = ImGui.GetIO();

        io.DisplaySize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        io.DisplayFramebufferScale = new Vector2(1, 1);
        io.DeltaTime = Raylib.GetFrameTime();

        io.KeyCtrl = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_CONTROL) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL);
        io.KeyShift = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT);
        io.KeyAlt = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_ALT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT);
        io.KeySuper = Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SUPER) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SUPER);

        if (io.WantSetMousePos)
        {
            Raylib.SetMousePosition((int)io.MousePos.X, (int)io.MousePos.Y);
        }
        else
        {
            io.MousePos = Raylib.GetMousePosition();
        }

        io.MouseDown[0] = Raylib.IsMouseButtonDown(Raylib.MOUSE_LEFT_BUTTON);
        io.MouseDown[1] = Raylib.IsMouseButtonDown(Raylib.MOUSE_LEFT_BUTTON);
        io.MouseDown[2] = Raylib.IsMouseButtonDown(Raylib.MOUSE_LEFT_BUTTON);

        if (Raylib.GetMouseWheelMove() > 0)
            io.MouseWheel += 1;
        else if (Raylib.GetMouseWheelMove() < 0)
            io.MouseWheel -= 1;


        if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0) return;
        ImGuiMouseCursor imguiCursor = ImGui.GetMouseCursor();
        if (imguiCursor == RlImGui.currentMouseCursor && !io.MouseDrawCursor) return;
        RlImGui.currentMouseCursor = imguiCursor;
        if (io.MouseDrawCursor || imguiCursor == ImGuiMouseCursor.None)
        {
            Raylib.HideCursor();
        }
        else
        {
            Raylib.ShowCursor();

            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) != 0) return;
            Raylib.SetMouseCursor(!RlImGui.mouseCursorMap.ContainsKey(imguiCursor)
                ? MouseCursor.MOUSE_CURSOR_DEFAULT
                : RlImGui.mouseCursorMap[imguiCursor]);
        }
    }


    private static void FrameEvents()
    {
        ImGuiIOPtr io = ImGui.GetIO();

        foreach (KeyboardKey key in Enum.GetValues(typeof(KeyboardKey)))
        {
            io.KeysDown[(int)key] = Raylib.IsKeyDown(key);
        }

        uint pressed = (uint)Raylib.GetCharPressed();
        while (pressed != 0)
        {
            io.AddInputCharacter(pressed);
            pressed = (uint)Raylib.GetCharPressed();
        }
    }

    public static void Begin()
    {
        ImGui.SetCurrentContext(RlImGui.ImGuiContext);

        RlImGui.NewFrame();
        RlImGui.FrameEvents();
        ImGui.NewFrame();
    }

    private static void EnableScissor(float x, float y, float width, float height)
    {
        
        RlGl.rlEnableScissorTest();
        RlGl.rlScissor((int)x, Raylib.GetScreenHeight() - (int)(y + height), (int)width, (int)height);
    }

    private static void TriangleVert(ImDrawVertPtr idxVert)
    {
        byte[] c = BitConverter.GetBytes(idxVert.col);

        RlGl.rlColor4ub(c[0], c[1], c[2], c[3]);

        RlGl.rlTexCoord2f(idxVert.uv.X, idxVert.uv.Y);
        RlGl.rlVertex2f(idxVert.pos.X, idxVert.pos.Y);
    }

    private static void RenderTriangles(uint count, uint indexStart, ImVector<ushort> indexBuffer, ImPtrVector<ImDrawVertPtr> vertBuffer, IntPtr texturePtr)
    {
        uint textureId = 0;
        if (texturePtr != IntPtr.Zero)
            textureId = (uint)texturePtr.ToInt32();

        RlGl.rlBegin(RlGl.RL_TRIANGLES);
        RlGl.rlSetTexture(textureId);

        for (int i = 0; i <= (count - 3); i += 3)
        {
            if (RlGl.rlCheckRenderBatchLimit(3))
            {
                RlGl.rlBegin(RlGl.RL_TRIANGLES);
                RlGl.rlSetTexture(textureId);
            }

            ushort indexA = indexBuffer[(int)indexStart + i];
            ushort indexB = indexBuffer[(int)indexStart + i + 1];
            ushort indexC = indexBuffer[(int)indexStart + i + 2];

            ImDrawVertPtr vertexA = vertBuffer[indexA];
            ImDrawVertPtr vertexB = vertBuffer[indexB];
            ImDrawVertPtr vertexC = vertBuffer[indexC];

            RlImGui.TriangleVert(vertexA);
            RlImGui.TriangleVert(vertexB);
            RlImGui.TriangleVert(vertexC);
        }
        RlGl.rlEnd();
    }

    private static void RenderData()
    {
        RlGl.rlDrawRenderBatchActive();
        RlGl.rlDisableBackfaceCulling();

        var data = ImGui.GetDrawData();

        for (int l = 0; l < data.CmdListsCount; l++)
        {
            uint idxOffset = 0;

            ImDrawListPtr commandList = data.CmdListsRange[l];

            for (int cmdIndex = 0; cmdIndex < commandList.CmdBuffer.Size; cmdIndex++)
            {
                var cmd = commandList.CmdBuffer[cmdIndex];

                RlImGui.EnableScissor(cmd.ClipRect.X - data.DisplayPos.X, cmd.ClipRect.Y - data.DisplayPos.Y, cmd.ClipRect.Z - (cmd.ClipRect.X - data.DisplayPos.X), cmd.ClipRect.W - (cmd.ClipRect.Y - data.DisplayPos.Y));
                if (cmd.UserCallback != IntPtr.Zero)
                {
                    //  cmd.UserCallback(commandList, &cmd);
                    idxOffset += cmd.ElemCount;
                    continue;
                }

                RlImGui.RenderTriangles(cmd.ElemCount, idxOffset, commandList.IdxBuffer, commandList.VtxBuffer, cmd.TextureId);
                idxOffset += cmd.ElemCount;

                RlGl.rlDrawRenderBatchActive();
            }
        }
        RlGl.rlSetTexture(0);
        RlGl.rlDisableScissorTest();
        RlGl.rlEnableBackfaceCulling();
    }

    public static void End()
    {
        ImGui.SetCurrentContext(RlImGui.ImGuiContext);
        ImGui.Render();
        RlImGui.RenderData();
    }

    public static void Shutdown()
    {
        Raylib.UnloadTexture(RlImGui.fontTexture);
    }

    public static void Image(Texture image)
    {
        ImGui.Image(new IntPtr(image.id), new Vector2(image.width, image.height));
    }

    public static void ImageSize(Texture image, int width, int height)
    {
        ImGui.Image(new IntPtr(image.id), new Vector2(width, height));
    }

    public static void ImageSize(Texture image, Vector2 size)
    {
        ImGui.Image(new IntPtr(image.id), size);
    }

    public static void ImageRect(Texture image, int destWidth, int destHeight, Rectangle sourceRect)
    {
        Vector2 uv0 = new Vector2();
        Vector2 uv1 = new Vector2();

        if (sourceRect.width < 0)
        {
            uv0.X = -(sourceRect.x / image.width);
            uv1.X = (uv0.X - Math.Abs(sourceRect.width) / image.width);
        }
        else
        {
            uv0.X = sourceRect.x / image.width;
            uv1.X = uv0.X + sourceRect.width / image.width;
        }

        if (sourceRect.height < 0)
        {
            uv0.Y = -(sourceRect.y / image.height);
            uv1.Y = (uv0.Y - Math.Abs(sourceRect.height) / image.height);
        }
        else
        {
            uv0.Y = sourceRect.y / image.height;
            uv1.Y = uv0.Y + sourceRect.height / image.height;
        }

        ImGui.Image(new IntPtr(image.id), new Vector2(destWidth, destHeight), uv0, uv1);
    }
}
