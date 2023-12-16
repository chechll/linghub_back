using AutoMapper;
using linghub.Dto;
using linghub.Interfaces;
using linghub.Repository;
using Microsoft.AspNetCore.Mvc;

namespace linghub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtextController : Controller
    {
        private readonly ITextRepository _textRepository;
        private readonly IU_textRepository _u_textRepository;
        private readonly IMapper _mapper;

        public UtextController(IU_textRepository u_textRepository,
            ITextRepository textRepository,
            IMapper mapper)
        {
            _textRepository = textRepository;
            _u_textRepository = u_textRepository;
            _mapper = mapper;
        }

        
    }
}
