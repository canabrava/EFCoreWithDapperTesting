using Dapper;
using System;
using System.Data;

namespace EFCoreWithDapper.Utils
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value) => parameter.Value = value;

        public override Guid Parse(object value) => Guid.Parse((string)value);
    }
}
