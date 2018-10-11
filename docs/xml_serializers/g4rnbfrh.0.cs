#if _DYNAMIC_XMLSERIALIZER_COMPILATION
[assembly:System.Security.AllowPartiallyTrustedCallers()]
[assembly:System.Security.SecurityTransparent()]
[assembly:System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]
#endif
[assembly:System.Reflection.AssemblyVersionAttribute("1.2.1.0")]
[assembly:System.Xml.Serialization.XmlSerializerVersionAttribute(ParentAssemblyId=@"e23af988-f873-4285-8272-3b05f7a47ccc,", Version=@"4.0.0.0")]
namespace Microsoft.Xml.Serialization.GeneratedAssembly {

    public class XmlSerializationWritermarket_descriptions : System.Xml.Serialization.XmlSerializationWriter {

        public void Write11_market_descriptions(object o) {
            WriteStartDocument();
            if (o == null) {
                WriteEmptyTag(@"market_descriptions", @"");
                return;
            }
            TopLevelElement();
            Write10_market_descriptions(@"market_descriptions", @"", ((global::Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions)o), false, false);
        }

        void Write10_market_descriptions(string n, string ns, global::Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            if (o.@response_codeSpecified) {
                WriteAttribute(@"response_code", @"", Write9_response_code(((global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code)o.@response_code)));
            }
            WriteAttribute(@"location", @"", ((global::System.String)o.@location));
            {
                global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market[] a = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market[])o.@market;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write8_desc_market(@"market", @"", ((global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market)a[ia]), false, false);
                    }
                }
            }
            if (o.@response_codeSpecified) {
            }
            WriteEndElement(o);
        }

        void Write8_desc_market(string n, string ns, global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(@"desc_market", @"");
            WriteAttribute(@"id", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@id)));
            WriteAttribute(@"name", @"", ((global::System.String)o.@name));
            WriteAttribute(@"groups", @"", ((global::System.String)o.@groups));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            WriteAttribute(@"includes_outcomes_of_type", @"", ((global::System.String)o.@includes_outcomes_of_type));
            WriteAttribute(@"variant", @"", ((global::System.String)o.@variant));
            {
                global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome[] a = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome[])((global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome[])o.@outcomes);
                if (a != null){
                    WriteStartElement(@"outcomes", @"", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write3_desc_outcomesOutcome(@"outcome", @"", ((global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier[] a = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier[])((global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier[])o.@specifiers);
                if (a != null){
                    WriteStartElement(@"specifiers", @"", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write4_desc_specifiersSpecifier(@"specifier", @"", ((global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping[] a = (global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping[])((global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping[])o.@mappings);
                if (a != null){
                    WriteStartElement(@"mappings", @"", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write6_mappingsMapping(@"mapping", @"", ((global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            {
                global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute[] a = (global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute[])((global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute[])o.@attributes);
                if (a != null){
                    WriteStartElement(@"attributes", @"", null, false);
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write7_attributesAttribute(@"attribute", @"", ((global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute)a[ia]), false, false);
                    }
                    WriteEndElement();
                }
            }
            WriteEndElement(o);
        }

        void Write7_attributesAttribute(string n, string ns, global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"name", @"", ((global::System.String)o.@name));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            WriteEndElement(o);
        }

        void Write6_mappingsMapping(string n, string ns, global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"product_id", @"", System.Xml.XmlConvert.ToString((global::System.Int32)((global::System.Int32)o.@product_id)));
            WriteAttribute(@"sport_id", @"", ((global::System.String)o.@sport_id));
            WriteAttribute(@"market_id", @"", ((global::System.String)o.@market_id));
            WriteAttribute(@"sov_template", @"", ((global::System.String)o.@sov_template));
            WriteAttribute(@"valid_for", @"", ((global::System.String)o.@valid_for));
            {
                global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome[] a = (global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome[])o.@mapping_outcome;
                if (a != null) {
                    for (int ia = 0; ia < a.Length; ia++) {
                        Write5_mappingsMappingMapping_outcome(@"mapping_outcome", @"", ((global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome)a[ia]), false, false);
                    }
                }
            }
            WriteEndElement(o);
        }

        void Write5_mappingsMappingMapping_outcome(string n, string ns, global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"outcome_id", @"", ((global::System.String)o.@outcome_id));
            WriteAttribute(@"product_outcome_id", @"", ((global::System.String)o.@product_outcome_id));
            WriteAttribute(@"product_outcome_name", @"", ((global::System.String)o.@product_outcome_name));
            WriteEndElement(o);
        }

        void Write4_desc_specifiersSpecifier(string n, string ns, global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"name", @"", ((global::System.String)o.@name));
            WriteAttribute(@"type", @"", ((global::System.String)o.@type));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            WriteEndElement(o);
        }

        void Write3_desc_outcomesOutcome(string n, string ns, global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome o, bool isNullable, bool needType) {
            if ((object)o == null) {
                if (isNullable) WriteNullTagLiteral(n, ns);
                return;
            }
            if (!needType) {
                System.Type t = o.GetType();
                if (t == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome)) {
                }
                else {
                    throw CreateUnknownTypeException(o);
                }
            }
            WriteStartElement(n, ns, o, false, null);
            if (needType) WriteXsiType(null, @"");
            WriteAttribute(@"id", @"", ((global::System.String)o.@id));
            WriteAttribute(@"name", @"", ((global::System.String)o.@name));
            WriteAttribute(@"description", @"", ((global::System.String)o.@description));
            WriteEndElement(o);
        }

        string Write9_response_code(global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code v) {
            string s = null;
            switch (v) {
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@OK: s = @"OK"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@CREATED: s = @"CREATED"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@ACCEPTED: s = @"ACCEPTED"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@FORBIDDEN: s = @"FORBIDDEN"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@NOT_FOUND: s = @"NOT_FOUND"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@CONFLICT: s = @"CONFLICT"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@SERVICE_UNAVAILABLE: s = @"SERVICE_UNAVAILABLE"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@NOT_IMPLEMENTED: s = @"NOT_IMPLEMENTED"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@MOVED_PERMANENTLY: s = @"MOVED_PERMANENTLY"; break;
                case global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@BAD_REQUEST: s = @"BAD_REQUEST"; break;
                default: throw CreateInvalidEnumValueException(((System.Int64)v).ToString(System.Globalization.CultureInfo.InvariantCulture), @"Sportradar.MTS.SDK.Entities.Internal.REST.response_code");
            }
            return s;
        }

        protected override void InitCallbacks() {
        }
    }

    public class XmlSerializationReadermarket_descriptions : System.Xml.Serialization.XmlSerializationReader {

        public object Read11_market_descriptions() {
            object o = null;
            Reader.MoveToContent();
            if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                if (((object) Reader.LocalName == (object)id1_market_descriptions && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o = Read10_market_descriptions(false, true);
                }
                else {
                    throw CreateUnknownNodeException();
                }
            }
            else {
                UnknownNode(null, @":market_descriptions");
            }
            return (object)o;
        }

        global::Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions Read10_market_descriptions(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions o;
            o = new global::Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions();
            global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market[] a_0 = null;
            int ca_0 = 0;
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id3_response_code && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@response_code = Read9_response_code(Reader.Value);
                    o.@response_codeSpecified = true;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id4_location && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@location = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":response_code, :location");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@market = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market[])ShrinkArray(a_0, ca_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations0 = 0;
            int readerCount0 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id5_market && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market[])EnsureArrayIndex(a_0, ca_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market));a_0[ca_0++] = Read8_desc_market(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":market");
                    }
                }
                else {
                    UnknownNode((object)o, @":market");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations0, ref readerCount0);
            }
            o.@market = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market[])ShrinkArray(a_0, ca_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market), true);
            ReadEndElement();
            return o;
        }

        global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market Read8_desc_market(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id6_desc_market && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market o;
            o = new global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_market();
            global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome[] a_0 = null;
            int ca_0 = 0;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier[] a_1 = null;
            int ca_1 = 0;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping[] a_2 = null;
            int ca_2 = 0;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute[] a_3 = null;
            int ca_3 = 0;
            bool[] paramsRead = new bool[10];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[4] && ((object) Reader.LocalName == (object)id7_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id8_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!paramsRead[6] && ((object) Reader.LocalName == (object)id9_groups && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@groups = Reader.Value;
                    paramsRead[6] = true;
                }
                else if (!paramsRead[7] && ((object) Reader.LocalName == (object)id10_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[7] = true;
                }
                else if (!paramsRead[8] && ((object) Reader.LocalName == (object)id11_includes_outcomes_of_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@includes_outcomes_of_type = Reader.Value;
                    paramsRead[8] = true;
                }
                else if (!paramsRead[9] && ((object) Reader.LocalName == (object)id12_variant && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@variant = Reader.Value;
                    paramsRead[9] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":id, :name, :groups, :description, :includes_outcomes_of_type, :variant");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations1 = 0;
            int readerCount1 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id13_outcomes && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome[] a_0_0 = null;
                            int ca_0_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations2 = 0;
                                int readerCount2 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id14_outcome && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_0_0 = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome[])EnsureArrayIndex(a_0_0, ca_0_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome));a_0_0[ca_0_0++] = Read3_desc_outcomesOutcome(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @":outcome");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @":outcome");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations2, ref readerCount2);
                                }
                            ReadEndElement();
                            }
                            o.@outcomes = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome[])ShrinkArray(a_0_0, ca_0_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome), false);
                        }
                    }
                    else if (((object) Reader.LocalName == (object)id15_specifiers && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier[] a_1_0 = null;
                            int ca_1_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations3 = 0;
                                int readerCount3 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id16_specifier && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_1_0 = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier[])EnsureArrayIndex(a_1_0, ca_1_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier));a_1_0[ca_1_0++] = Read4_desc_specifiersSpecifier(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @":specifier");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @":specifier");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations3, ref readerCount3);
                                }
                            ReadEndElement();
                            }
                            o.@specifiers = (global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier[])ShrinkArray(a_1_0, ca_1_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier), false);
                        }
                    }
                    else if (((object) Reader.LocalName == (object)id17_mappings && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping[] a_2_0 = null;
                            int ca_2_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations4 = 0;
                                int readerCount4 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id18_mapping && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_2_0 = (global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping[])EnsureArrayIndex(a_2_0, ca_2_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping));a_2_0[ca_2_0++] = Read6_mappingsMapping(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @":mapping");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @":mapping");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations4, ref readerCount4);
                                }
                            ReadEndElement();
                            }
                            o.@mappings = (global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping[])ShrinkArray(a_2_0, ca_2_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping), false);
                        }
                    }
                    else if (((object) Reader.LocalName == (object)id19_attributes && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        if (!ReadNull()) {
                            global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute[] a_3_0 = null;
                            int ca_3_0 = 0;
                            if ((Reader.IsEmptyElement)) {
                                Reader.Skip();
                            }
                            else {
                                Reader.ReadStartElement();
                                Reader.MoveToContent();
                                int whileIterations5 = 0;
                                int readerCount5 = ReaderCount;
                                while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                                    if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                                        if (((object) Reader.LocalName == (object)id20_attribute && (object) Reader.NamespaceURI == (object)id2_Item)) {
                                            a_3_0 = (global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute[])EnsureArrayIndex(a_3_0, ca_3_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute));a_3_0[ca_3_0++] = Read7_attributesAttribute(false, true);
                                        }
                                        else {
                                            UnknownNode(null, @":attribute");
                                        }
                                    }
                                    else {
                                        UnknownNode(null, @":attribute");
                                    }
                                    Reader.MoveToContent();
                                    CheckReaderCount(ref whileIterations5, ref readerCount5);
                                }
                            ReadEndElement();
                            }
                            o.@attributes = (global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute[])ShrinkArray(a_3_0, ca_3_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute), false);
                        }
                    }
                    else {
                        UnknownNode((object)o, @":outcomes, :specifiers, :mappings, :attributes");
                    }
                }
                else {
                    UnknownNode((object)o, @":outcomes, :specifiers, :mappings, :attributes");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations1, ref readerCount1);
            }
            ReadEndElement();
            return o;
        }

        global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute Read7_attributesAttribute(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute o;
            o = new global::Sportradar.MTS.SDK.Entities.Internal.REST.attributesAttribute();
            bool[] paramsRead = new bool[2];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id8_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id10_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":name, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations6 = 0;
            int readerCount6 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations6, ref readerCount6);
            }
            ReadEndElement();
            return o;
        }

        global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping Read6_mappingsMapping(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping o;
            o = new global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMapping();
            global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome[] a_0 = null;
            int ca_0 = 0;
            bool[] paramsRead = new bool[6];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[1] && ((object) Reader.LocalName == (object)id21_product_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@product_id = System.Xml.XmlConvert.ToInt32(Reader.Value);
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id22_sport_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@sport_id = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!paramsRead[3] && ((object) Reader.LocalName == (object)id23_market_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@market_id = Reader.Value;
                    paramsRead[3] = true;
                }
                else if (!paramsRead[4] && ((object) Reader.LocalName == (object)id24_sov_template && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@sov_template = Reader.Value;
                    paramsRead[4] = true;
                }
                else if (!paramsRead[5] && ((object) Reader.LocalName == (object)id25_valid_for && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@valid_for = Reader.Value;
                    paramsRead[5] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":product_id, :sport_id, :market_id, :sov_template, :valid_for");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                o.@mapping_outcome = (global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome[])ShrinkArray(a_0, ca_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome), true);
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations7 = 0;
            int readerCount7 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    if (((object) Reader.LocalName == (object)id26_mapping_outcome && (object) Reader.NamespaceURI == (object)id2_Item)) {
                        a_0 = (global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome[])EnsureArrayIndex(a_0, ca_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome));a_0[ca_0++] = Read5_mappingsMappingMapping_outcome(false, true);
                    }
                    else {
                        UnknownNode((object)o, @":mapping_outcome");
                    }
                }
                else {
                    UnknownNode((object)o, @":mapping_outcome");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations7, ref readerCount7);
            }
            o.@mapping_outcome = (global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome[])ShrinkArray(a_0, ca_0, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome), true);
            ReadEndElement();
            return o;
        }

        global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome Read5_mappingsMappingMapping_outcome(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome o;
            o = new global::Sportradar.MTS.SDK.Entities.Internal.REST.mappingsMappingMapping_outcome();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id27_outcome_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@outcome_id = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id28_product_outcome_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@product_outcome_id = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id29_product_outcome_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@product_outcome_name = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":outcome_id, :product_outcome_id, :product_outcome_name");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations8 = 0;
            int readerCount8 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations8, ref readerCount8);
            }
            ReadEndElement();
            return o;
        }

        global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier Read4_desc_specifiersSpecifier(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier o;
            o = new global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_specifiersSpecifier();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id8_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id30_type && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@type = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id10_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":name, :type, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations9 = 0;
            int readerCount9 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations9, ref readerCount9);
            }
            ReadEndElement();
            return o;
        }

        global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome Read3_desc_outcomesOutcome(bool isNullable, bool checkType) {
            System.Xml.XmlQualifiedName xsiType = checkType ? GetXsiType() : null;
            bool isNull = false;
            if (isNullable) isNull = ReadNull();
            if (checkType) {
            if (xsiType == null || ((object) ((System.Xml.XmlQualifiedName)xsiType).Name == (object)id2_Item && (object) ((System.Xml.XmlQualifiedName)xsiType).Namespace == (object)id2_Item)) {
            }
            else
                throw CreateUnknownTypeException((System.Xml.XmlQualifiedName)xsiType);
            }
            if (isNull) return null;
            global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome o;
            o = new global::Sportradar.MTS.SDK.Entities.Internal.REST.desc_outcomesOutcome();
            bool[] paramsRead = new bool[3];
            while (Reader.MoveToNextAttribute()) {
                if (!paramsRead[0] && ((object) Reader.LocalName == (object)id7_id && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@id = Reader.Value;
                    paramsRead[0] = true;
                }
                else if (!paramsRead[1] && ((object) Reader.LocalName == (object)id8_name && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@name = Reader.Value;
                    paramsRead[1] = true;
                }
                else if (!paramsRead[2] && ((object) Reader.LocalName == (object)id10_description && (object) Reader.NamespaceURI == (object)id2_Item)) {
                    o.@description = Reader.Value;
                    paramsRead[2] = true;
                }
                else if (!IsXmlnsAttribute(Reader.Name)) {
                    UnknownNode((object)o, @":id, :name, :description");
                }
            }
            Reader.MoveToElement();
            if (Reader.IsEmptyElement) {
                Reader.Skip();
                return o;
            }
            Reader.ReadStartElement();
            Reader.MoveToContent();
            int whileIterations10 = 0;
            int readerCount10 = ReaderCount;
            while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.NodeType != System.Xml.XmlNodeType.None) {
                if (Reader.NodeType == System.Xml.XmlNodeType.Element) {
                    UnknownNode((object)o, @"");
                }
                else {
                    UnknownNode((object)o, @"");
                }
                Reader.MoveToContent();
                CheckReaderCount(ref whileIterations10, ref readerCount10);
            }
            ReadEndElement();
            return o;
        }

        global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code Read9_response_code(string s) {
            switch (s) {
                case @"OK": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@OK;
                case @"CREATED": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@CREATED;
                case @"ACCEPTED": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@ACCEPTED;
                case @"FORBIDDEN": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@FORBIDDEN;
                case @"NOT_FOUND": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@NOT_FOUND;
                case @"CONFLICT": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@CONFLICT;
                case @"SERVICE_UNAVAILABLE": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@SERVICE_UNAVAILABLE;
                case @"NOT_IMPLEMENTED": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@NOT_IMPLEMENTED;
                case @"MOVED_PERMANENTLY": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@MOVED_PERMANENTLY;
                case @"BAD_REQUEST": return global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code.@BAD_REQUEST;
                default: throw CreateUnknownConstantException(s, typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.response_code));
            }
        }

        protected override void InitCallbacks() {
        }

        string id11_includes_outcomes_of_type;
        string id4_location;
        string id18_mapping;
        string id27_outcome_id;
        string id14_outcome;
        string id22_sport_id;
        string id19_attributes;
        string id25_valid_for;
        string id23_market_id;
        string id15_specifiers;
        string id26_mapping_outcome;
        string id17_mappings;
        string id13_outcomes;
        string id1_market_descriptions;
        string id3_response_code;
        string id21_product_id;
        string id9_groups;
        string id20_attribute;
        string id10_description;
        string id16_specifier;
        string id28_product_outcome_id;
        string id29_product_outcome_name;
        string id6_desc_market;
        string id5_market;
        string id2_Item;
        string id7_id;
        string id30_type;
        string id24_sov_template;
        string id8_name;
        string id12_variant;

        protected override void InitIDs() {
            id11_includes_outcomes_of_type = Reader.NameTable.Add(@"includes_outcomes_of_type");
            id4_location = Reader.NameTable.Add(@"location");
            id18_mapping = Reader.NameTable.Add(@"mapping");
            id27_outcome_id = Reader.NameTable.Add(@"outcome_id");
            id14_outcome = Reader.NameTable.Add(@"outcome");
            id22_sport_id = Reader.NameTable.Add(@"sport_id");
            id19_attributes = Reader.NameTable.Add(@"attributes");
            id25_valid_for = Reader.NameTable.Add(@"valid_for");
            id23_market_id = Reader.NameTable.Add(@"market_id");
            id15_specifiers = Reader.NameTable.Add(@"specifiers");
            id26_mapping_outcome = Reader.NameTable.Add(@"mapping_outcome");
            id17_mappings = Reader.NameTable.Add(@"mappings");
            id13_outcomes = Reader.NameTable.Add(@"outcomes");
            id1_market_descriptions = Reader.NameTable.Add(@"market_descriptions");
            id3_response_code = Reader.NameTable.Add(@"response_code");
            id21_product_id = Reader.NameTable.Add(@"product_id");
            id9_groups = Reader.NameTable.Add(@"groups");
            id20_attribute = Reader.NameTable.Add(@"attribute");
            id10_description = Reader.NameTable.Add(@"description");
            id16_specifier = Reader.NameTable.Add(@"specifier");
            id28_product_outcome_id = Reader.NameTable.Add(@"product_outcome_id");
            id29_product_outcome_name = Reader.NameTable.Add(@"product_outcome_name");
            id6_desc_market = Reader.NameTable.Add(@"desc_market");
            id5_market = Reader.NameTable.Add(@"market");
            id2_Item = Reader.NameTable.Add(@"");
            id7_id = Reader.NameTable.Add(@"id");
            id30_type = Reader.NameTable.Add(@"type");
            id24_sov_template = Reader.NameTable.Add(@"sov_template");
            id8_name = Reader.NameTable.Add(@"name");
            id12_variant = Reader.NameTable.Add(@"variant");
        }
    }

    public abstract class XmlSerializer1 : System.Xml.Serialization.XmlSerializer {
        protected override System.Xml.Serialization.XmlSerializationReader CreateReader() {
            return new XmlSerializationReadermarket_descriptions();
        }
        protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter() {
            return new XmlSerializationWritermarket_descriptions();
        }
    }

    public sealed class market_descriptionsSerializer : XmlSerializer1 {

        public override System.Boolean CanDeserialize(System.Xml.XmlReader xmlReader) {
            return xmlReader.IsStartElement(@"market_descriptions", @"");
        }

        protected override void Serialize(object objectToSerialize, System.Xml.Serialization.XmlSerializationWriter writer) {
            ((XmlSerializationWritermarket_descriptions)writer).Write11_market_descriptions(objectToSerialize);
        }

        protected override object Deserialize(System.Xml.Serialization.XmlSerializationReader reader) {
            return ((XmlSerializationReadermarket_descriptions)reader).Read11_market_descriptions();
        }
    }

    public class XmlSerializerContract : global::System.Xml.Serialization.XmlSerializerImplementation {
        public override global::System.Xml.Serialization.XmlSerializationReader Reader { get { return new XmlSerializationReadermarket_descriptions(); } }
        public override global::System.Xml.Serialization.XmlSerializationWriter Writer { get { return new XmlSerializationWritermarket_descriptions(); } }
        System.Collections.Hashtable readMethods = null;
        public override System.Collections.Hashtable ReadMethods {
            get {
                if (readMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions:::False:"] = @"Read11_market_descriptions";
                    if (readMethods == null) readMethods = _tmp;
                }
                return readMethods;
            }
        }
        System.Collections.Hashtable writeMethods = null;
        public override System.Collections.Hashtable WriteMethods {
            get {
                if (writeMethods == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp[@"Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions:::False:"] = @"Write11_market_descriptions";
                    if (writeMethods == null) writeMethods = _tmp;
                }
                return writeMethods;
            }
        }
        System.Collections.Hashtable typedSerializers = null;
        public override System.Collections.Hashtable TypedSerializers {
            get {
                if (typedSerializers == null) {
                    System.Collections.Hashtable _tmp = new System.Collections.Hashtable();
                    _tmp.Add(@"Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions:::False:", new market_descriptionsSerializer());
                    if (typedSerializers == null) typedSerializers = _tmp;
                }
                return typedSerializers;
            }
        }
        public override System.Boolean CanSerialize(System.Type type) {
            if (type == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions)) return true;
            return false;
        }
        public override System.Xml.Serialization.XmlSerializer GetSerializer(System.Type type) {
            if (type == typeof(global::Sportradar.MTS.SDK.Entities.Internal.REST.market_descriptions)) return new market_descriptionsSerializer();
            return null;
        }
    }
}
