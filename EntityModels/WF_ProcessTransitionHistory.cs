//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class WF_ProcessTransitionHistory
    {
        public System.Guid Id { get; set; }
        public System.Guid ProcessId { get; set; }
        public string fromNodeId { get; set; }
        public Nullable<int> fromNodeType { get; set; }
        public string fromNodeName { get; set; }
        public string toNodeId { get; set; }
        public Nullable<int> toNodeType { get; set; }
        public string toNodeName { get; set; }
        public int TransitionSate { get; set; }
        public int isFinish { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserName { get; set; }
    }
}
