namespace RPG.Control
{
    public interface IRaycastable
    {
        Cursor_Type Get_Cursor_Type();
        bool Handle_Raycast(Player_Controller call_controller);
    }
}