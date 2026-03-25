using QuanLyPhongTro.Models;
using QuanLyPhongTro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongTro.Services
{
    public class OccupantService : IOccupantService
    {
        private readonly IOccupantRepository _occupantRepository;
        private readonly IContractRepository _contractRepository;

        public OccupantService(IOccupantRepository occupantRepository, IContractRepository contractRepository)
        {
            _occupantRepository = occupantRepository;
            _contractRepository = contractRepository;
        }

        public IEnumerable<Occupant> GetOccupantsByContract(int contractId)
        {
            return _occupantRepository.GetOccupantsByContractId(contractId);
        }

        public bool AddOccupant(Occupant occupant, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Kiểm tra rỗng
            if(string.IsNullOrWhiteSpace(occupant.FullName) || string.IsNullOrWhiteSpace(occupant.IdentifyCard))
            {
                errorMessage = "Họ tên và CCCD không được để trống";
                return false;
            }

            // Lấy thông tin Hợp đồng (kèm thông tin phòng) để kiểm tra sức chứa
            var contract = _contractRepository.GetContractWithDetails(occupant.ContractId);
            if(contract == null || !contract.IsActive)
            {
                errorMessage = "Hợp đồng không tồn tại hoặc đã hết hiệu lực";
                return false;
            }

            // Đếm số người Hiện tại đang có trong hợp đồng này
            var currentOccupantsCount = _occupantRepository.GetOccupantsByContractId(occupant.ContractId).Count();

            // Kiểm tra xem thêm người này vào có vượt MaxOccupant không
            if(currentOccupantsCount >= contract.Room.MaxOccupants)
            {
                errorMessage = $"Phòng này chỉ tối đa {contract.Room.MaxOccupants}. Không thể thêm người mới!";
                return false;
            }

            // Kiểm tra trùng CCCD
            var existingByIdentity = _occupantRepository.GetOccupantByIdentityCard(occupant.IdentifyCard);
            if(existingByIdentity != null)
            {
                errorMessage = $"CCCD {occupant.IdentifyCard} đã được đăng kí trong hệ thống!";
                return false;
            }

            _occupantRepository.Add(occupant);
            _occupantRepository.SaveChanges();
            return true;
        }

        public bool RemoveOccupant(int occupantId, out string errorMessage)
        {
            errorMessage = string.Empty;
            var occupant = _occupantRepository.GetById(occupantId);

            if(occupant == null)
            {
                errorMessage = "Không tìm thấy dữ liệu nhân khẩu!";
                return false;
            }

            // Không cho phép xóa người đứng tên hợp đồng
            if (occupant.IsContractOwner)
            {
                errorMessage = "Không thể xóa người đại diện đứng tên hợp đồng. Nếu người này rời đi vui lòng THANH LÝ hợp đồng";
                return false;
            }
            _occupantRepository.Delete(occupant);
            _occupantRepository.SaveChanges();
            return true;
        }
    }
}
