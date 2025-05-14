using BeautyFromNature3.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BeautyFromNature3.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<BeautyUser> _userManager;
        private readonly SignInManager<BeautyUser> _signInManager;

        public IndexModel(
            UserManager<BeautyUser> userManager,
            SignInManager<BeautyUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Потребителското име е задължително.")]
            [MaxLength(20)]
            [Display(Name = "Потребителско име")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Името е задължително.")]
            [MaxLength(20)]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Фамилията е задължителна.")]
            [MaxLength(40)]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Имейлът е задължителен.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Телефонният номер е задължителен.")]
            [Phone]
            [Display(Name = "Телефонен номер")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(BeautyUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не може да се зареди потребител с ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Неочаквана грешка при опит за задаване на телефонен номер."; //Unexpected error when trying to set phone number.
                    return RedirectToPage();
                }
            }

            if (Input.UserName != user.UserName)
            {
                user.UserName = Input.UserName;
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }
            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }
            if (Input.Email != user.Email)
            {
                user.Email = Input.Email;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Вашият профил е актуализиран";
            return RedirectToPage();
        }
    }
}
