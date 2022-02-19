using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace MeuProjeto.Models
{
    public class NHibertnateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
            var usersConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\TUsers.hbm.xml");
            configuration.AddFile(usersConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}