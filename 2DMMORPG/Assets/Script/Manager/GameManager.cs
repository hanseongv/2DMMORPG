using Script.Utils;

namespace Script
{
    public class GameManager : MSingleton<GameManager>
    {
        private readonly ResoureceManager _resoureceManager=new ResoureceManager();
        public static ResoureceManager ResoureceM => Instance._resoureceManager;
    }
}