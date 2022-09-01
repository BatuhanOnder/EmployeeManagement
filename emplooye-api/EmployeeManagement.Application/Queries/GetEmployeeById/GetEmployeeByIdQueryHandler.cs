﻿namespace EmployeeManagement.Application.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, GetEmployeeByIdQueryResponse>
{
    private readonly IEmployeeService _service;

    public GetEmployeeByIdQueryHandler(IEmployeeService service)
    {
        _service = service;
    }

    public async Task<GetEmployeeByIdQueryResponse> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _service.GetEmployeeById(request.Id);
        return response;
        
    }
}