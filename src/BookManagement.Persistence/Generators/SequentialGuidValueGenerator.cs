using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookManagement.Persistence.Generators;

public sealed class SequentialGuidValueGenerator : ValueGenerator<Guid>
{
    public override bool GeneratesTemporaryValues => false;

    public override Guid Next(EntityEntry entry)
    {
        // Emulate SequentialGuid using timestamp
        var guidBytes = Guid.NewGuid().ToByteArray();
        var timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(timestamp);

        // Replace last 6 bytes with timestamp
        Array.Copy(timestamp, 2, guidBytes, 10, 6);
        return new Guid(guidBytes);
    }
}
