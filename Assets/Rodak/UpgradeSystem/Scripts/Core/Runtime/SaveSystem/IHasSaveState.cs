using Newtonsoft.Json.Linq;

namespace Rodak.UpgradeSystem.Core
{
    public interface IHasSaveState
    {
        public JObject CreateSaveState();
        public void LoadSaveState(JObject saveStateObject);
    }
}