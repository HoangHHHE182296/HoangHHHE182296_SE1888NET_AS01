using Presentation.Models;

namespace Presentation.View_Model.Data {
    public class AccountViewModel {
        public AccountModel Account { get; set; }
        public List<NewsArticleModel> Articles { get; set; }
    }
}
