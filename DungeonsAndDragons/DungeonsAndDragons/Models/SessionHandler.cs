namespace StaticHttpContextAccessor.Helpers
{
    public class SessionHandler
    {

        public bool UserIsSignedIn()
        {
            return AppContext.Current.Session.GetS("username") != "null";
        }



        public void SetUserSession(string username, int id)
        {
            AppContext.Current.Session.SetS("username", username);
            AppContext.Current.Session.SetInt("userID", id);
        }



        public string GetSignedInUsername()
        {
            return AppContext.Current.Session.GetS("username");
        }



        public int GetSignedInUserID()
        {
            return AppContext.Current.Session.GetInt("userID") ?? default(int);
        }
    }
}