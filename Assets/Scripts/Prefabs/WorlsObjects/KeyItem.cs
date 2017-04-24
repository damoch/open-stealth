
namespace Assets.Scripts.Prefabs.WorlsObjects
{
    public class KeyItem : Item
    {
        public KeyItem(string name)
        {
            KeyCode = name;
        }


        public override bool IsThisKeyCode(string code)
        {
            return KeyCode.Equals(code);
        } 
    }
}
