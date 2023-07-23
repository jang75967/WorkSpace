using System;

namespace VideoAppCore.Models
{
    /// <summary>
    /// AuditableBase 클래스: 레코드에 대한 상태 추적을 위한 4개의 속성 제공
    /// ex) Dul.dll -> Dul.Domain.Common.AuditableBase.cs 
    /// </summary>
    public class AuditableBase
    {
        /// <summary>
        /// 등록자: CreatedBy, Creator
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 등록일: Created
        /// </summary>
        //public DateTimeOffset Created { get; set; }
        public DateTime Created { get; set; }

        /// <summary>
        /// 수정자: LastModifiedBy, ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 수정일: LastModified, Modified
        /// </summary>
        public DateTime? Modified { get; set; }
    }
}
