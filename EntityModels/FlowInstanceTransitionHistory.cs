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
    
    public partial class FlowInstanceTransitionHistory
    {
        public string Id { get; set; }
        public string InstanceId { get; set; }
        public string FromNodeId { get; set; }
        public Nullable<int> FromNodeType { get; set; }
        public string FromNodeName { get; set; }
        public string ToNodeId { get; set; }
        public Nullable<int> ToNodeType { get; set; }
        public string ToNodeName { get; set; }
        public int TransitionSate { get; set; }
        public int IsFinish { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUserId { get; set; }
        public string CreateUserName { get; set; }
    }
}