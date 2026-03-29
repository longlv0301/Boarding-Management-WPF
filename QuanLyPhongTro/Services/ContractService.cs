using QuanLyPhongTro.Models;
using QuanLyPhongTro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ITenantRepository _tenantRepository;

        public ContractService(IContractRepository contractRepository, IRoomRepository roomRepository, ITenantRepository tenantRepository)
        {
            _contractRepository = contractRepository;
            _roomRepository = roomRepository;
            _tenantRepository = tenantRepository;
        }

        public IEnumerable<Contract> GetAllContracts()
        {
            return _contractRepository.GetAll();
        }

        public IEnumerable<Contract> GetActiveContracts()
        {
            return _contractRepository.GetActiveContracts();
        }

        public Contract GetContractDetails(int contractId)
        {
            return _contractRepository.GetContractWithDetails(contractId);
        }

        public bool CreateContract(Contract contract, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Kiểm tra ngày tháng hợp lệ
            if(contract.StartDate > contract.EndDate)
            {
                errorMessage = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!";
                return false;
            }

            if(contract.DepositAmout < 0)
            {
                errorMessage = "Tiền cọc không được là số âm!";
                return false;
            }

            // Lấy thông tin phòng lên để kiểm tra
            var room = _roomRepository.GetById(contract.RoomId);
            if(room == null)
            {
                errorMessage = "Phòng không tồn tại";
                return false;
            }

            // Phòng trống mới lập được hợp đồng
            if(room.Status != RoomStatus.Available)
            {
                errorMessage = $"Phòng {room.Name} hiện đang không trống. Không thể lập hợp đồng";
                return false;
            }

            var tenant = _tenantRepository.GetById(contract.TenantId);
            if(tenant == null)
            {
                errorMessage = "Không tìm thấy khách thuê!";
                return false;
            }

            contract.Occupants = new List<Occupant>()
            {
                new Occupant
                {
                    FullName = tenant.FullName,
                    IdentifyCard = tenant.IdentityCard,
                    PhoneNumber = tenant.PhoneNumber,
                    LicensePlate = tenant.LicensePlate,
                    IsContractOwner = true
                }
            };

            contract.IsActive = true;
            _contractRepository.Add(contract);

            room.Status = RoomStatus.Rented;
            _roomRepository.Update(room);

            _contractRepository.SaveChanges();
            return true;
        }

        public bool TerminateContract(int contractId, out string errorMessage)
        {
            errorMessage = string.Empty;

            var contract = _contractRepository.GetById(contractId);
            if(contract == null || !contract.IsActive)
            {
                errorMessage = "Hợp đồng không tồn tại hoặc được thanh lý từ trước";
                return false;
            }

            // 1. Đánh dấu hợp đồng hết hiệu lực
            contract.IsActive = false;
            _contractRepository.Update(contract);

            // 2. Tìm phòng của hợp đồng này để trả về trạng thái trống
            var room = _roomRepository.GetById(contract.RoomId);
            if(room != null)
            {
                room.Status = RoomStatus.Available;
                _roomRepository.Update(room);
            }

            _contractRepository.SaveChanges();
            return true;
        }
    }
}
