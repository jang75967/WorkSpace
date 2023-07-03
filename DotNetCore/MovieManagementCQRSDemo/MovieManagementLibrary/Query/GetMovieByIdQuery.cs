using MediatR;
using MovieManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagementLibrary.Query
{
    // 쿼리 시 처리할 서비스가 결정
    // Key인 id로 찾으므로 List 아닌 단일 모델
    public record GetMovieByIdQuery(int id) : IRequest<MovieModel>;
}
