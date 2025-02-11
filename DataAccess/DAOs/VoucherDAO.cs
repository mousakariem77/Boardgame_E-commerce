using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DAOs;

public class VoucherDAO : SingletonBase<VoucherDAO>
{
    public async Task<List<Voucher>> GetAllVouchersAsync()
    {
        return await _context.Vouchers.ToListAsync();
    }

    public async Task<Voucher> GetVoucherByIdAsync(long voucherId)
    {
        return await _context.Vouchers.FindAsync(voucherId);
    }

    public async Task<Voucher> CreateVoucherAsync(Voucher voucher)
    {
        if (voucher == null) throw new ArgumentNullException(nameof(voucher));
        await _context.Vouchers.AddAsync(voucher);
        await _context.SaveChangesAsync();
        return voucher;
    }

    public async Task<Voucher> UpdateVoucherAsync(Voucher voucher)
    {
        var existingVoucher = await _context.Vouchers.FindAsync(voucher.VoucherId);
        if (existingVoucher != null)
        {
            existingVoucher.VoucherCode = voucher.VoucherCode;
            existingVoucher.VoucherName = voucher.VoucherName;
            existingVoucher.VoucherType = voucher.VoucherType;
            existingVoucher.VoucherDiscount = voucher.VoucherDiscount;
            existingVoucher.VoucherStartAt = voucher.VoucherStartAt;
            existingVoucher.VoucherEndAt = voucher.VoucherEndAt;
            existingVoucher.VoucherMax = voucher.VoucherMax;
            existingVoucher.VoucherQuantity = voucher.VoucherQuantity;
            existingVoucher.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return existingVoucher;
        }

        return null;
    }

    public async Task DeleteVoucherAsync(long voucherId)
    {
        var voucher = await _context.Vouchers.FindAsync(voucherId);
        if (voucher != null)
        {
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
        }
    }
}